using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
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
		//private IGenericRepository<IdentityRole> _UserRole;
		
		#endregion

		#region Public IGenericRepository Get Methods with new Instantiations

		public IGenericRepository<ApplicationUser> ApplicationUser
		{
			get
			{
				//Assigns a value only if it is null
				_ApplicationUser ??= new GenericRepository<ApplicationUser>(_dbContext);
				return _ApplicationUser;
			}
		}
	

		//public IGenericRepository<IdentityRole> UserRole
		//{
		//	get
		//	{
		//		_UserRole ??= new GenericRepository<IdentityRole>(_dbContext);
		//		return _UserRole;
		//	}
		//}
		
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

