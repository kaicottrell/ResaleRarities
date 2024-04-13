using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(ApplicationDbContext dbContext)
        {
            // Apply any pending migrations
            dbContext.Database.Migrate();


            // Check if the database is already seeded
            if (!dbContext.ListingStatus.Any())
            {
                // Database has been seeded before
                dbContext.ListingStatus.AddRange(
                     new ListingStatus { StatusDescription = SD.LSDraft },
                     new ListingStatus { StatusDescription = SD.LSPostedForBid },
                     new ListingStatus { StatusDescription = SD.LSDeclinedAutomatedOffer },
                     new ListingStatus { StatusDescription = SD.LSProcessingAutomatedOffer },
                     new ListingStatus { StatusDescription = SD.LSPostedAutomatedOffer },
                     new ListingStatus { StatusDescription = SD.LSNoBidsMade }
                 );

                dbContext.SaveChanges();

            }
            // Check if the database is already seeded
            if (!dbContext.Condition.Any())
            {
                // Database has been seeded before
                dbContext.Condition.AddRange(
                    new Condition { Type = SD.NewCT },
                    new Condition { Type = SD.LikeNewCT },
                    new Condition { Type = SD.UsedCT },
                    new Condition { Type = SD.DamagedCT }
                );


                dbContext.SaveChanges();

            }

            if (!dbContext.Category.Any())
            {
                // Database has been seeded before
                dbContext.Category.AddRange(
                    new Category { Name = "Electronics & Computers" },
                    new Category { Name = "Home & Garden" },
                    new Category { Name = "Clothing, Shoes & Accessories" },
                    new Category { Name = "Toys & Hobbies" },
                    new Category { Name = "Health & Beauty" },
                    new Category { Name = "Sporting Goods" },
                    new Category { Name = "Collectibles & Art" },
                    new Category { Name = "Musical Instruments & Gear" },
                    new Category { Name = "Books" },
                    new Category { Name = "Business & Industrial" },
                    new Category { Name = "Cameras & Photo" },
                    new Category { Name = "Cell Phones & Accessories" },
                    new Category { Name = "Pet Supplies" },
                    new Category { Name = "Baby" },
                    new Category { Name = "Travel" },
                    new Category { Name = "Crafts" },
                    new Category { Name = "Jewelry & Watches" },
                    new Category { Name = "Antiques" },
                    new Category { Name = "Coins & Paper Money" },
                    new Category { Name = "DVDs & Movies" },
                    new Category { Name = "Music" },
                    new Category { Name = "Video Games & Consoles" }
                    // Add more categories as needed
                );

                dbContext.SaveChanges();


                dbContext.SaveChanges();
            }



        }
    }
}
