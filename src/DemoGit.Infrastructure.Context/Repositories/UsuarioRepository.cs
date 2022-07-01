using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;
using DemoGit.Infrastructure.Context.Interfaces;
using DemoGit.Security.Argon2;
using MongoDB.Driver;

namespace DemoGit.Infrastructure.Context.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Usuario> _collection;

        public UsuarioRepository(DatabaseContext context) :
        base(context)
        {
            _database = context.GetDatabase("DemoGit");
            _collection = _database.GetCollection<Usuario>("Usuario");
        }

        public Usuario SelectByUsername(string? username)
        {
            return _collection.Find(u => u.Username == username).FirstOrDefault();
        }

        public override async void Create(Usuario entity)
        {
            entity.HashPassword = Argon2Utils.HashPassword(entity.Password!);

            await _collection.InsertOneAsync(entity);
        }
    }
}