using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;
using DemoGit.Infrastructure.Context.Interfaces;
using MongoDB.Driver;

namespace DemoGit.Infrastructure.Context.Repositories
{
    public class ResourceRepository : Repository<Resource>, IResourceRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Resource> _collection;

        public ResourceRepository(DatabaseContext context)
        : base(context)
        {
            _database = context.GetDatabase("DemoGit");
            _collection = _database.GetCollection<Resource>("Resources");
        }

        public List<Resource> GetImages()
        {
            return _collection.Find(r => r.ResourceType == "image").ToList();
        }

        public Resource SelectByFileName(string fileName)
        {
            return _collection.Find(r => r.FileName == fileName).FirstOrDefault();
        }
    }
}