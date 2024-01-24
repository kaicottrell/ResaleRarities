using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
	public interface IUnitofWork
	{
		#region IGenericRepository Properties
		// Alphabetized for ease of access
		public IGenericRepository<ApplicationUser> ApplicationUser { get; }
        public IGenericRepository<Category> Category { get; }
        public IGenericRepository<Condition> Condition { get; }
        public IGenericRepository<Inventory> Inventory { get; }
        public IGenericRepository<Listing> Listing { get; }
        public IGenericRepository<ListingStatus> ListingStatus { get; }
        public IGenericRepository<Order> Order { get; }
        public IGenericRepository<OrderStatus> OrderStatus { get; }
        public IGenericRepository<Product> Product { get; }
        public IGenericRepository<TokenInfo> TokenInfo { get; }
        public IGenericRepository<UserRole> UserRole { get; }

        #endregion

        // saves changes to data source
        int Commit();

		// copy of above for Async
		Task<int> CommitAsync();
	}
}
