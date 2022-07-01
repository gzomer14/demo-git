using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Infrastructure.Context.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DemoGit.Infrastructure.Context.Repositories
{
    public class Repository<T> : IRepository<T>
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<T> _collection;

        public Repository(DatabaseContext context)
        {
            _database = context.GetDatabase("DemoGit");
            _collection = _database.GetCollection<T>(typeof(T).Name);
        }

        public virtual async void Create(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async void DeleteById(string id)
        {
            await _collection.DeleteOneAsync(FilterById(id));
        }

        public List<T> SelectAll()
        {
            return _collection.Find(new BsonDocument()).ToList();
        }

        public T SelectById(string id)
        {
            return _collection.Find(FilterById(id)).FirstOrDefault();
        }

        public virtual async void Update(T entity)
        {
            var idValue = entity?.GetType()?.GetProperty("Id")?.GetValue(entity, null)?.ToString() ?? "";

            await _collection.ReplaceOneAsync(FilterById(idValue), entity);
        }

        private FilterDefinition<T> FilterById(string id)
        {
            return new BsonDocument("_id", id);
        }
    }
}