﻿using System.Collections.Generic;
using System.Linq.Expressions;
using HouseBuildingFinanceWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseBuildingFinanceWebApp.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public virtual async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

        public virtual void Remove(T entity) => _dbSet.Remove(entity);

        public virtual void Update(T entity) => _dbSet.Update(entity);
    }
}
