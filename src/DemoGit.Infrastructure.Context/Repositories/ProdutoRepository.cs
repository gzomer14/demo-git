using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;
using DemoGit.Infrastructure.Context.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DemoGit.Infrastructure.Context.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Produto> _collection;

    public ProdutoRepository(DatabaseContext context)
    {
        _database = context.GetDatabase("DemoGit");
        _collection = _database.GetCollection<Produto>("Produto");
    }

    public void Create(Produto entity)
    {
        _collection.InsertOne(entity);
    }

    public void DeleteById(string id)
    {
        _collection.DeleteOne(opt => opt.Id == id);
    }

    public List<Produto> SelectAll()
    {
        return _collection.Find(new BsonDocument()).ToList();
    }

    public Produto SelectById(string id)
    {
        return _collection.Find(opt => opt.Id == id).FirstOrDefault();
    }

    public void Update(Produto entity)
    {
        _collection.ReplaceOne(opt => opt.Id == entity.Id, entity);
    }
}
