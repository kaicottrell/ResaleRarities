using OpenAI_API;
using System;
using System.Threading.Tasks;
using ApplicationCore.Models;
using OpenAI_API.Completions;
using OpenAI_API.Chat;
using ApplicationCore.Models.ViewModels;
using System.Text.RegularExpressions;
using OpenAI_API.Models;
using Microsoft.Extensions.Configuration;

namespace ResaleRarities.Pages.Services
{
    public class AutomatedOfferService
    {
        OpenAIAPI _openAiApi;
        private readonly IConfiguration _config;
        public AutomatedOfferService(IConfiguration config)
        {
            _config = config;
            _openAiApi = new OpenAIAPI("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"); 
        }

        Product ProductObj = new Product();
        public async Task<OfferProduct> GetOfferWithReasonTest(Product product)
        {
            double bidPercentage = 0.6;
            double offerAmount = 5 * bidPercentage;
            string reason = $"Bid: ${offerAmount}. We're interested in your {product.Name}, especially since it's in {product.Condition.Type} condition. {product.Name} is known for its quality, and we'd like to make an offer for your item.";

            return new OfferProduct(product.Id, offerAmount, reason);
        }
        public async Task<OfferProduct> GetOfferWithReason(Product product)
        {
            ProductObj = product;

            var chat = _openAiApi.Chat.CreateConversation();
            chat.Model = Model.GPT4;

            /// give instruction as System
            chat.AppendSystemMessage("You are an automated bidding system. You bid 60% of what the price of the item would be based on the description, name, condition, and category."+
               "You start with the double value bid and then give your reasoning. Utilize this format: Bid:BID_VARIABLE: STRING_REASONING. Where the underscore words are the corresponding variables");

            // give a few examples as user and assistant
            chat.AppendUserInput("Create an automated bid for: Samsung TV. Description: It's in great working condition, barely used since I got it. Category: Electronics. Condition: Like New.");
            chat.AppendExampleChatbotOutput("Bid: $350. We're interested in your Samsung TV, especially since it's in like-new condition. Samsung is known for its quality, and we'd like to make an offer for your item.");

            chat.AppendUserInput("Make an automated bid for: Vintage Rolex Watch. Description: Authentic Rolex watch from the 1960s, in excellent condition. Category: Jewelry & Watches. Condition: Like New.");
            chat.AppendExampleChatbotOutput("Offer: $5000. Your vintage Rolex watch sounds like a rare find, especially in excellent condition. We're interested in purchasing it for our collection.");


            string prompt = $"Create an automated bid for: {product.Name}. Description: {product.Description}. Category: {product.Category?.Name}. Condition: {product.Condition?.Type}.";
           // now let's ask it a question
            chat.AppendUserInput(prompt);
            // and get the response
            string response = await chat.GetResponseFromChatbotAsync();
            Console.WriteLine(response); // "Yes"

            response = await chat.GetResponseFromChatbotAsync();
 
            var offerResult = ProcessCompletionWithReason(response);

            return offerResult;
        }

        private OfferProduct ProcessCompletionWithReason(string response)
        {
            double offer = 0.0;
            string reason = "";

            // Define the regular expression pattern to match the offer amount
            string offerPattern = @"Bid: \$([\d.]+)";

            // Use regular expression to match the offer amount
            Match offerMatch = Regex.Match(response, offerPattern);

            // Check if the offer amount is matched
            if (offerMatch.Success)
            {
                // Extract and parse the offer amount
                if (double.TryParse(offerMatch.Groups[1].Value, out offer))
                {
                    // Extract the reason as the substring after the offer
                    int reasonIndex = offerMatch.Index + offerMatch.Length;
                    reason = response.Substring(reasonIndex).Trim();

                    // Return the offer product
                    return new OfferProduct(ProductObj.Id, offer, reason);
                }
            }

            // If no offer is found, throw an exception
            throw new Exception("Unable to extract offer and reason from completion response.");
        }


    }
}
