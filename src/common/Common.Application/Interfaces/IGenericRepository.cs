﻿using System.Linq.Expressions;

namespace Common.Application.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    // Create
    void Add(TEntity entity);
    Task AddAsync(TEntity entity);

    // Read
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    TEntity FindOne(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null, CancellationToken cancellationToken = default);

    // Update
    void Update(TEntity entity);

    // Delete
    void Delete(TEntity? entity);

    // Saving
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class FindOptions
{
    public bool IsIgnoreAutoIncludes { get; set; }
    public bool IsAsNoTracking { get; set; }
}