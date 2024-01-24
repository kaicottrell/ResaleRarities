using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Data
{
	public class UnitofWork : IUnitofWork
	{
		#region UnitofWork Constructor
		private readonly ApplicationDbContext _dbContext;

		public UnitofWork(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		#endregion

		#region Private IGenericRepository Variables  
		private IGenericRepository<ApplicationUser> _ApplicationUser;
        private IGenericRepository<Category> _Category;
        private IGenericRepository<Condition> _Condition;
        private IGenericRepository<Inventory> _Inventory;
        private IGenericRepository<Listing> _Listing;
        private IGenericRepository<ListingStatus> _ListingStatus;
        private IGenericRepository<Order> _Order;
        private IGenericRepository<OrderStatus> _OrderStatus;
        private IGenericRepository<Product> _Product;
        private IGenericRepository<TokenInfo> _TokenInfo;
        private IGenericRepository<UserRole> _UserRole;


        //private IGenericRepository<IdentityRole> _UserRole;

        #endregion

        #region Public IGenericRepository Get Methods with new Instantiations

        public IGenericRepository<ApplicationUser> ApplicationUser
        {
            get
            {
                //Assigns a value only if it is null, if not return null
                _ApplicationUser ??= new GenericRepository<ApplicationUser>(_dbContext);
                return _ApplicationUser;
            }
        }
        public IGenericRepository<Category> Category
        {
            get
            {
                _Category ??= new GenericRepository<Category>(_dbContext);
                return _Category;
            }
        }
        public IGenericRepository<Condition> Condition
        {
            get
            {
                _Condition ??= new GenericRepository<Condition>(_dbContext);
                return _Condition;
            }
        }
        public IGenericRepository<Inventory> Inventory
        {
            get
            {
                _Inventory ??= new GenericRepository<Inventory>(_dbContext);
                return _Inventory;
            }
        }
        public IGenericRepository<Listing> Listing
        {
            get
            {
                _Listing ??= new GenericRepository<Listing>(_dbContext);
                return _Listing;
            }
        }
        public IGenericRepository<ListingStatus> ListingStatus
        {
            get
            {
                _ListingStatus ??= new GenericRepository<ListingStatus>(_dbContext);
                return _ListingStatus;
            }
        }
        public IGenericRepository<Order> Order
        {
            get
            {
                _Order ??= new GenericRepository<Order>(_dbContext);
                return _Order;
            }
        }
        public IGenericRepository<OrderStatus> OrderStatus
        {
            get
            {
                _OrderStatus ??= new GenericRepository<OrderStatus>(_dbContext);
                return _OrderStatus;
            }
        }
        public IGenericRepository<Product> Product
        {
            get
            {
                _Product ??= new GenericRepository<Product>(_dbContext);
                return _Product;
            }
        }
        public IGenericRepository<TokenInfo> TokenInfo
        {
            get
            {
                _TokenInfo ??= new GenericRepository<TokenInfo>(_dbContext);
                return _TokenInfo;
            }
        }

        public IGenericRepository<UserRole> UserRole
        {
            get
            {
                _UserRole ??= new GenericRepository<UserRole>(_dbContext);
                return _UserRole;
            }
        }

        #endregion

        #region Public Commit Methods
        public int Commit()
		{
			return _dbContext.SaveChanges();
		}
		public async Task<int> CommitAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}
		#endregion

		#region Public Dispose Method
		// Doesn't need to inherit off the interface, it's just the minimum
		public void Dispose()
		{
			_dbContext.Dispose();
		}
		#endregion

	}
}

