using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DemoGit.Infrastructure.Context.Interfaces;

public interface IRepository<T>
{
    void Create(T entity);
    void Update(T entity);
    void DeleteById(string id);
    List<T> SelectAll();
    T SelectById(string id);
    List<T> Find(Expression<Func<T, bool>> filter);
}
