using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace UserRolesAPI.Core.Interfaces.Data;

public interface IRepository : IDisposable
{
    void RollBack();
    Task RollBackAsync();
    DbContext GetDbContext();
    T Add<T>(T t, [CallerMemberName] string callerName = "") where T : class;
    List<T> Add<T>(List<T> t, [CallerMemberName] string callerName = "") where T : class;
    Task<T> AddAsync<T>(T t, [CallerMemberName] string callerName = "") where T : class;
    Task<List<T>> AddAsyn<T>(List<T> t, [CallerMemberName] string callerName = "") where T : class;
    int Count<T>() where T : class;
    Task<int> CountAsync<T>() where T : class;
    void Delete<T>(object id) where T : class;
    void Delete<T>(T entity) where T : class;
    void Delete<T>(List<T> t) where T : class;
    T Find<T>(Expression<Func<T, bool>> match) where T : class;
    T First<T>(Expression<Func<T, bool>> match) where T : class;
    Task<T> FirstAsync<T>(Expression<Func<T, bool>> match) where T : class;
    Task<T> FirstOrderBy<T>(Expression<Func<T, object>> order) where T : class;
    ICollection<T> FindAll<T>(Expression<Func<T, bool>> match) where T : class;
    Task<List<T>> FindAllAsync<T>(Expression<Func<T, bool>> match) where T : class;
    bool Any<T>(Expression<Func<T, bool>> match) where T : class;
    Task<bool> AnyAsync<T>(Expression<Func<T, bool>> match) where T : class;
    ICollection<T> FindTake<T>(Expression<Func<T, bool>> match, Expression<Func<T, object>> order, int take, bool asc = true) where T : class;
    Task<List<T>> FindTakeAsync<T>(Expression<Func<T, bool>> match, Expression<Func<T, object>> order, int take, bool asc = true) where T : class;
    Task<T> FindAsync<T>(Expression<Func<T, bool>> match) where T : class;
    IQueryable<T> FindBy<T>(Expression<Func<T, bool>> predicate) where T : class;
    Task<List<T>> FindByAsyn<T>(Expression<Func<T, bool>> predicate) where T : class;
    IQueryable<T> GetAll<T>() where T : class;
    Task<List<T>> GetAllAsync<T>() where T : class;
    IQueryable<T> GetAllIncluding<T>(params Expression<Func<T, object>>[] includeProperties) where T : class;
    T Update<T>(T t, object key, [CallerMemberName] string callerName = "") where T : class;
    Task<T> UpdateAsync<T>(T t, object key, [CallerMemberName] string callerName = "") where T : class;
    T AddOrUpdate<T>(T t, object key, [CallerMemberName] string callerName = "") where T : class;
    Task<T> AddOrUpdateAsync<T>(T t, object key, [CallerMemberName] string callerName = "") where T : class;
    void AddOrUpdateDate();
    void SaveChanges();
    Task<int> SaveChangesAsync(bool compensateNegativeBalance = true);
}
