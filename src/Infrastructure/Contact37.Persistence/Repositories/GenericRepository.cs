using Contacts37.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact37.Persistence.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly Contact37DbContext _dbcontext;
		public GenericRepository(Contact37DbContext context)
        {
			this._dbcontext = context;
		}

        public async Task<T> Add(T entity)
		{
			await _dbcontext.AddAsync(entity);
			await _dbcontext.SaveChangesAsync();
			return entity;
		}

		public async Task Delete(T entity)
		{
			_dbcontext.Set<T>().Remove(entity);
			await _dbcontext.SaveChangesAsync();
		}

		public async Task<bool> Exists(Guid id)
		{
			var entity = await Get(id);
			return entity != null;
		}

		public async Task<T> Get(Guid id)
		{
			return await _dbcontext.Set<T>().FindAsync(id);
		}

		public async Task<IReadOnlyList<T>> GetAll()
		{
			return await _dbcontext.Set<T>().ToListAsync();
		}

		public async Task Update(T entity)
		{
			_dbcontext.Entry(entity).State = EntityState.Modified;
			await _dbcontext.SaveChangesAsync();
		}
	}
}
