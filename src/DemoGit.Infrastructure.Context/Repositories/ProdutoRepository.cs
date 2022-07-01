using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;
using DemoGit.Infrastructure.Context.Interfaces;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DemoGit.Infrastructure.Context.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Produto> _collection;

    public ProdutoRepository(DatabaseContext context)
    : base(context)
    {
        _database = context.GetDatabase("DemoGit");
        _collection = _database.GetCollection<Produto>("Produto");
    }

    public List<Produto> SelectLikeDescription(string pesquisa)
    {
        return _collection.Find(p => p.Descricao!.ToLower().Contains(pesquisa.ToLower())).ToList();
    }

    public override async void Update(Produto entity)
    {
        var idFilter = new BsonDocument("_id", entity.Id);
        var oldProduct = (await _collection.FindAsync(idFilter)).FirstOrDefault();

        if (entity.Imagem is null && oldProduct.Imagem is not null)
            entity.Imagem = oldProduct.Imagem;

        await _collection.ReplaceOneAsync(idFilter, entity);
    }
}
