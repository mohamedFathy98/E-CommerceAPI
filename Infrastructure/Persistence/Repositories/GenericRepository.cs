﻿using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreContext _storeContext;

        public GenericRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public async Task AddAsync(TEntity entity) => await _storeContext.Set<TEntity>().AddAsync(entity);


        public void Delete(TEntity entity) => _storeContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => _storeContext.Set<TEntity>().Update(entity);
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChange = false) =>

                 //if (trackChange)
                 //return await _storeContext.Set<TEntity>().ToListAsync();
                 //return await _storeContext.Set<TEntity>().AsNoTracking().ToListAsync();

                 trackChange ? await _storeContext.Set<TEntity>().ToListAsync()
                : await _storeContext.Set<TEntity>().AsNoTracking().ToListAsync();



        public async Task<TEntity?> GetAsync(TKey id) => await _storeContext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity?> GetAsync(Specifications<TEntity> specifications)
               =>
                   await ApplySpecifications(specifications).FirstOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(Specifications<TEntity> specifications)
        =>
           await ApplySpecifications(specifications).ToListAsync();

        private IQueryable<TEntity> ApplySpecifications(Specifications<TEntity> specifications)
       => SpecificationEvaluator.GetQuery<TEntity>(_storeContext.Set<TEntity>(), specifications);
    }
}
