using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utility
{
	//THIS CLASS IS FOR STATIC DEFINTIONS i.e. status names, flag types, identifiers, etc.
	public class SD
	{
        #region ListingStatuses
        public const string LSDraft = "Draft";
        public const string LSPostedForBid = "Posted for bid";
        public const string LSDeclinedAutomatedOffer = "Declined automated offer";
        public const string LSProcessingAutomatedOffer = "Submitted for automated offer";
        public const string LSAcceptedAutomaticOffer = "Accepted Automatic Offer";
        public const string LSPostedAutomatedOffer = "Automated offer made to user";
        public const string LSNoBidsMade= "No bids made";

        #endregion
        #region ConditionTypes
        public const string NewCT = "New (In Original Packaging)";
        public const string LikeNewCT = "Like New (Lightly Used)";
        public const string UsedCT = "Used (Good Condition)";
        public const string DamagedCT = "Damaged (Visible Wear or Damage)";
        #endregion
    }
}
