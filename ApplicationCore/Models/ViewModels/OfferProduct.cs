using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models.ViewModels
{
    public class OfferProduct
    {
       
        public string ProductId { get; set; }
        public double OfferAmount { get; set; }
        public string Reason { get; set; }
        public OfferProduct(string productId, double offerAmount, string reason)
        {
            ProductId = productId;
            OfferAmount = offerAmount;
            Reason = reason;
        }
        
    }
}
