using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Models.ViewModels;
using Infrastructure.Services;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Components;
using OpenAI_API;
using ResaleRarities.Authentication;
using ResaleRarities.Pages.Services;
using System.Security.Claims;

namespace ResaleRarities.Pages.Listings
{
    public partial class ListingCrum
    {
        [Parameter]
        public string? ListingId { get; set; }
        public string? SelectedProductId { get; set; }
        private Listing Listing { get; set; } = new Listing();
        private IEnumerable<Product> Products { get; set; } = new List<Product>();
        private int SelectedListingOptionId { get; set; } = 0;

        private string ProductName { get; set; } = string.Empty;
        private string ProductDescription { get; set; } = string.Empty;
        //Booleans relating to the visibility of the modals
        private bool IsProductModalVisible { get; set; } = false;
        /// <summary>
        /// Selecting Listing Type, pairs with listing status id
        /// </summary>
        private Dictionary<string, int> ListingOptions { get; set; } = new Dictionary<string, int>();
        [Inject]
        private IUnitofWork? _unitofWork { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; } // Inject NavigationManager
        [Inject]
        private AutomatedOfferService OfferService { get; set; }
        private bool ShowOffer { get; set; } = false; // Flag to track Opening the offer
        private List<OfferProduct> OfferedProducts { get; set; } = new List<OfferProduct>();

        private bool IsDataLoaded { get; set; } = false; // Flag to track data loading



        // Basically a Onintialized when parameters are used
        protected override async Task OnParametersSetAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            IsDataLoaded = false;
            var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
            var authState = await customAuthStateProvider.GetAuthenticationStateAsync();
            var userName = authState.User.FindFirst(ClaimTypes.Email).Value;
            var user = _unitofWork.ApplicationUser.Get(u => u.Email == userName);

            if (!string.IsNullOrEmpty(ListingId) && _unitofWork != null)
            {
                Listing = _unitofWork.Listing.Get(listing => listing.Id == ListingId);
            }
            else if (_unitofWork != null && user != null)
            {
                var draftLS = _unitofWork.ListingStatus.Get(ls => ls.StatusDescription == SD.LSDraft);
                var newListing = new Listing
                {
                    ListingStatusId = draftLS.Id,
                    ApplicationUserId = user.Id
                };
                _unitofWork.Listing.Add(newListing);
                _unitofWork.Commit();
                Listing = newListing;
            }
            else
            {
                //TODO: throw error
            }
            Products = _unitofWork!.Product.List(product => product.ListingId == Listing.Id, includes:"Category,Condition");

            //Get the listing status id of the listing option associated with posting for online bid
            var bidLSId = _unitofWork.ListingStatus.Get(u => u.StatusDescription == SD.LSPostedForBid).Id;
            //Get the listing status id associated with submitting for an automatted offer
            var automatedLSId = _unitofWork.ListingStatus.Get(ls => ls.StatusDescription == SD.LSProcessingAutomatedOffer).Id;

            ListingOptions.Add("Submit for Online Bid", bidLSId);
            ListingOptions.Add("Submit for Automated Offer", automatedLSId);


            IsDataLoaded = true; // Set the flag to true after data is loaded

            StateHasChanged(); // Trigger re-rendering of the component
        }



        private async void HandleList()
        {
            bool allProductsComplete = Products
            .All(product =>
                !string.IsNullOrEmpty(product.Category?.Name) && !string.IsNullOrEmpty(product.Condition?.Type)
                && _unitofWork.Image.Get(image => image.ProductId == product.Id) != null
             );

            bool IsListingTypeSelected = SelectedListingOptionId != 0;

            if (allProductsComplete && IsListingTypeSelected)
            {
                // All products have category and condition

                Listing.ListingStatusId = _unitofWork.ListingStatus.Get(status => status.Id == SelectedListingOptionId).Id;

                _unitofWork.Listing.Update(Listing);
                _unitofWork.Commit();
                NotificationService.ShowNotification("SUCCESS: Your Listing Has Been Posted", "success");

                try
                {
                    double totalOffer = 0;
                    foreach (var Product in Products)
                    {
                        var offerProduct = await OfferService.GetOfferWithReasonTest(Product);
                        double offer = offerProduct.OfferAmount;
                        OfferedProducts.Add(offerProduct);
                        totalOffer += offer;
                    }
                    Listing.Compensation = (decimal)totalOffer;
                }
                catch (Exception ex)
                {
                    // Handle the exception gracefully, such as logging or displaying an error message
                    Console.WriteLine($"An error occurred while adding offers to OfferedProducts: {ex.Message}");
                }
                _unitofWork.Listing.Update(Listing);
                _unitofWork.Commit();
                ShowOffer = true;
                StateHasChanged();
                // Navigation.NavigateTo("/");
            }
            else if (!allProductsComplete)
            {
                // Not all products have complete information
                NotificationService.ShowNotification("ERROR: All Product Details Must Be Complete Before Listing", "error");
                Listing.ListingStatusId = _unitofWork.ListingStatus.Get(status => status.StatusDescription == SD.LSDraft).Id;
            }
            else if (!IsListingTypeSelected)
            {
                // Not all products have complete information
                NotificationService.ShowNotification("ERROR: Listing Type Must Be Selected", "error");
                Listing.ListingStatusId = _unitofWork.ListingStatus.Get(status => status.StatusDescription == SD.LSDraft).Id;
            }
            // Handle form submission
            _unitofWork.Listing.Update(Listing);
            _unitofWork.Commit();
        }

        // Method to show the product modal
        private void ShowProductModal(string? productId = null)
        {
            // Set IsVisible to true to show the modal
            IsProductModalVisible = true;
            SelectedProductId = productId;
        }
        private void UpdateVisibility(bool isVisible)
        {
            IsProductModalVisible = isVisible;
        }
        private void HandleProductAdded()
        {
            Products = _unitofWork!.Product.List(product => product.ListingId == Listing.Id);
            IsProductModalVisible = false; // Close the modal after adding the product
        }
        private void ExitListing()
        {
            // Navigate to the previous URL
            Navigation.NavigateTo("/");
        }
        private void DeleteListingAndExit()
        {
            _unitofWork.Product.Delete(Products);
            _unitofWork.Listing.Delete(Listing);
            Navigation.NavigateTo("/");
        }

        private void HandleCloseModal()
        {
            IsProductModalVisible = false;
            Products = _unitofWork!.Product.List(product => product.ListingId == Listing.Id);

        }
        private void RemoveListing()
        {
            Listing.ListingStatusId = _unitofWork.ListingStatus.Get(status => status.StatusDescription == SD.LSDraft).Id;
            _unitofWork.Listing.Update(Listing);
            NotificationService.ShowNotification("Sucessfully Removed Posting", "success");
            StateHasChanged();
        }


    }
}