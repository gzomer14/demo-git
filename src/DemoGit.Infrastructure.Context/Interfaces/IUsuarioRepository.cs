using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;

namespace DemoGit.Infrastructure.Context.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario SelectByUsername(string? username);
        void Create(Usuario entity, string webUrl);
        void EnviarEmailEsqueciSenha(Usuario user, string webUrl);
        void Update(Usuario user, string newPassword);
    }
}