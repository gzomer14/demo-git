using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;
using DemoGit.Domain.Interfaces;
using DemoGit.Infrastructure.Context.Interfaces;
using DemoGit.Security.Argon2;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DemoGit.Infrastructure.Context.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Usuario> _collection;
        private readonly IEmailService _emailService;

        public UsuarioRepository(DatabaseContext context, IEmailService emailService) :
        base(context)
        {
            _database = context.GetDatabase("DemoGit");
            _collection = _database.GetCollection<Usuario>("Usuario");
            _emailService = emailService;
        }

        public Usuario SelectByUsername(string? username)
        {
            return _collection.Find(u => u.Username == username).FirstOrDefault();
        }

        public async void Create(Usuario entity, string webUrl)
        {
            entity.HashPassword = Argon2Utils.HashPassword(entity.Password!);

            await _collection.InsertOneAsync(entity);

            var body = @$"<h1>Conta de usuário {entity.Username} criada com sucesso!</h1>
                          <p>Parabéns, {entity.FullName}, você acaba de criar sua conta no melhor site da internet atualmente!</p>
                          <p>Para acessar sua nova conta, <a href='{webUrl + "/Login"}'><b>Clique Aqui</b></a></p>";

            _emailService.SendEmail(entity.Email!, "Conta criada com sucesso", body);
        }

        public void EnviarEmailEsqueciSenha(Usuario user, string webUrl)
        {
            var body = @$"<h1>Esqueci minha senha!</h1>
                         <p>Você acabou esquecendo sua senha? Não tem problema!!</p>
                         <p><a href='{webUrl + "/Login/RedefinirSenha?username=" + user.Username}'><b>Clique Aqui</b></a> para redefini-lá.</p>";

            _emailService.SendEmail(user.Email!, "Esqueci minha senha", body);
        }

        public async void Update(Usuario user, string newPassword)
        {
            user.HashPassword = Argon2Utils.HashPassword(newPassword);

            await _collection.ReplaceOneAsync(new BsonDocument("_id", user.Id), user);
        }
    }
}