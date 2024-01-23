using Microsoft.AspNetCore.Identity;
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


		#endregion

		// saves changes to data source
		int Commit();

		// copy of above for Async
		Task<int> CommitAsync();
	}
}
