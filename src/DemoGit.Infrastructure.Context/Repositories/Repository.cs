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

        public void Create(T entity)
        {
            _collection.InsertOne(entity);
        }

        public void DeleteById(string id)
        {
            _collection.DeleteOne(FilterById(id));
        }

        public List<T> SelectAll()
        {
            return _collection.Find(new BsonDocument()).ToList();
        }

        public T SelectById(string id)
        {
            return _collection.Find(FilterById(id)).FirstOrDefault();
        }

        public void Update(T entity)
        {
            var idValue = entity?.GetType()?.GetProperty("Id")?.GetValue(entity, null)?.ToString() ?? "";

            _collection.ReplaceOne(FilterById(idValue), entity);
        }

        private FilterDefinition<T> FilterById(string id)
        {
            return new BsonDocument("_id", new ObjectId(id));
        }
    }
}