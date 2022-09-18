using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserRolesAPI.Core.Interfaces.Data;

namespace UserRolesAPI.Infrastructure.Data.Repositories;

public class Repository : IRepository
{
    private readonly DbContext _context;

    public Repository(AppDbContext dbContext)
    {
        _context = dbContext;
    }
    public DbContext GetDbContext()
    {
        return _context;
    }

    public void RollBack()
    {
        var changedEntries = _context.ChangeTracker.Entries()
             .Where(x => x.State != EntityState.Unchanged).ToList();

        foreach (var entry in changedEntries)
        {
            // reset context changes
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }

            // reload db values
            entry.Reload();
        }
    }

    public async Task RollBackAsync()
    {
        var changedEntries = _context.ChangeTracker.Entries()
            .Where(x => x.State != EntityState.Unchanged).ToList();

        foreach (var entry in changedEntries)
        {
            //reset context changes
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }

            // reload db values
            await entry.ReloadAsync();
        }
    }

    public void SaveChanges()
    {
        AddOrUpdateDate();
        _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(bool compensateNegativeBalance = true)
    {
        int saveResult;

        AddOrUpdateDate();

        saveResult = await _context.SaveChangesAsync();

        return saveResult;
    }

    public async Task<T> FirstOrderBy<T>(Expression<Func<T, object>> order) where T : class
    {
        return await _context.Set<T>().OrderByDescending(order).FirstAsync();
    }

    public void AddOrUpdateDate()
    {
        // Created datetime
        var addedEntities = _context.ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();
        var now = DateTime.UtcNow;

        addedEntities.ForEach(E =>
        {
            var exists = E.Entity.GetType().GetProperties().FirstOrDefault(x => x.Name == "Created");

            if (exists != null)
                E.Property("Created").CurrentValue = now;

            var existsUpdate = E.Entity.GetType().GetProperties().FirstOrDefault(x => x.Name == "Updated");
            if (existsUpdate != null)
                E.Property("Updated").CurrentValue = now;
        });

        // Updated datetime
        var editedEntities = _context.ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

        editedEntities.ForEach(E =>
        {
            var exists = E.Entity.GetType().GetProperties().FirstOrDefault(x => x.Name == "Updated");
            if (exists != null)
                E.Property("Updated").CurrentValue = now;
        });
    }

    public IQueryable<T> GetAll<T>() where T : class
    {
        return _context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync<T>() where T : class
    {
        return await _context.Set<T>().ToListAsync();
    }

    public T Add<T>(T t, [CallerMemberName] string callerName = "") where T : class
    {
        var entryEntity = _context.Set<T>().Add(t);
        return entryEntity.Entity;
    }

    public List<T> Add<T>(List<T> t, [CallerMemberName] string callerName = "") where T : class
    {
        var list = new List<T>();
        foreach (var item in t)
        {
            var entryEntity = _context.Set<T>().Add(item);
            list.Add(entryEntity.Entity);
        }

        return list;
    }

    public async Task<T> AddAsync<T>(T t, [CallerMemberName] string callerName = "") where T : class
    {
        var entryEntity = await _context.Set<T>().AddAsync(t);
        return entryEntity.Entity;
    }

    public async Task<List<T>> AddAsyn<T>(List<T> t, [CallerMemberName] string callerName = "") where T : class
    {
        var list = new List<T>();
        foreach (var item in t)
        {
            var entryEntity = await _context.Set<T>().AddAsync(item);
            list.Add(entryEntity.Entity);
        }

        return list;
    }

    public T Find<T>(Expression<Func<T, bool>> match) where T : class
    {
        return _context.Set<T>().SingleOrDefault(match);
    }

    public async Task<T> FindAsync<T>(Expression<Func<T, bool>> match) where T : class
    {
        return await _context.Set<T>().SingleOrDefaultAsync(match);
    }

    public T First<T>(Expression<Func<T, bool>> match) where T : class
    {
        return _context.Set<T>().FirstOrDefault(match);
    }

    public async Task<T> FirstAsync<T>(Expression<Func<T, bool>> match) where T : class
    {
        return await _context.Set<T>().FirstOrDefaultAsync(match);
    }

    public ICollection<T> FindAll<T>(Expression<Func<T, bool>> match) where T : class
    {
        return _context.Set<T>().Where(match).ToList();
    }

    public async Task<List<T>> FindAllAsync<T>(Expression<Func<T, bool>> match) where T : class
    {
        return await _context.Set<T>().Where(match).ToListAsync();
    }

    public bool Any<T>(Expression<Func<T, bool>> match) where T : class
    {
        return _context.Set<T>().Any(match);
    }

    public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> match) where T : class
    {
        return await _context.Set<T>().AnyAsync(match);
    }

    public ICollection<T> FindTake<T>(Expression<Func<T, bool>> match, Expression<Func<T, object>> order, int take, bool asc = true) where T : class
    {
        if (asc)
        {
            return _context.Set<T>().Where(match).OrderBy(order).Take(take).ToList();
        }
        return _context.Set<T>().OrderByDescending(order).Where(match).Take(take).ToList();
    }

    public async Task<List<T>> FindTakeAsync<T>(Expression<Func<T, bool>> match, Expression<Func<T, object>> order, int take, bool asc = true) where T : class
    {
        if (asc)
        {
            return await _context.Set<T>().Where(match).OrderBy(order).Take(take).ToListAsync();
        }
        return await _context.Set<T>().Where(match).OrderByDescending(order).Take(take).ToListAsync();
    }

    public void Delete<T>(object id) where T : class
    {
        var entityToDelete = _context.Set<T>().Find(id);
        Delete<T>(entityToDelete);
    }

    public void Delete<T>(T entityToDelete) where T : class
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
        {
            _context.Set<T>().Attach(entityToDelete);
        }
        _context.Set<T>().Remove(entityToDelete);
    }

    public void Delete<T>(List<T> t) where T : class
    {
        _context.RemoveRange(t);
    }

    public T AddOrUpdate<T>(T t, object key, [CallerMemberName] string callerName = "") where T : class
    {
        var exist = _context.Set<T>().Find(key);
        if (exist != null)
        {
            _context.Entry(exist).CurrentValues.SetValues(t);
            return exist;
        }
        else
        {
            return Add(t);
        }
    }

    public async Task<T> AddOrUpdateAsync<T>(T t, object key, [CallerMemberName] string callerName = "") where T : class
    {
        var exist = await _context.Set<T>().FindAsync(key);
        if (exist != null)
        {
            var entries = _context.ChangeTracker.Entries();
            var currentValues = _context.Entry(exist).CurrentValues;
            _context.Entry(exist).CurrentValues.SetValues(t);
            return t;
        }
        else
        {
            return await AddAsync(t);
        }
    }

    public T Update<T>(T t, object key, [CallerMemberName] string callerName = "") where T : class
    {
        if (t == null)
            return null;
        var exist = _context.Set<T>().Find(key);
        if (exist != null)
        {
            _context.Entry(exist).CurrentValues.SetValues(t);
            _context.Entry(exist).State = EntityState.Modified;
        }
        return exist;
    }

    public async Task<T> UpdateAsync<T>(T t, object key, [CallerMemberName] string callerName = "") where T : class
    {
        if (t == null)
            return null;
        var exist = await _context.Set<T>().FindAsync(key);
        if (exist != null)
        {
            _context.Entry(exist).CurrentValues.SetValues(t);
            _context.Entry(exist).State = EntityState.Modified;
        }
        return exist;
    }

    public int Count<T>() where T : class
    {
        return _context.Set<T>().Count();
    }

    public async Task<int> CountAsync<T>() where T : class
    {
        return await _context.Set<T>().CountAsync();
    }

    public IQueryable<T> FindBy<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        var query = _context.Set<T>().Where(predicate);
        return query;
    }

    public async Task<List<T>> FindByAsyn<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public IQueryable<T> GetAllIncluding<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
    {

        var queryable = GetAll<T>();
        foreach (var includeProperty in includeProperties)
        {
            queryable = queryable.Include(includeProperty);
        }

        return queryable;
    }

    private bool _disposed;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
