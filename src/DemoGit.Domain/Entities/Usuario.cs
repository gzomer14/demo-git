using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DemoGit.Domain.Entities
{
    public class Usuario
    {
        public Usuario()
        {
            Id = ObjectId.GenerateNewId().ToString();
            Ativo = true;
        }

        [BsonId]
        public string? Id { get; set; }

        [Required(ErrorMessage = "O campo usuário é obrigatório!")]
        [Display(Name = "Usuário")]
        public string? Username { get; set; }

        [BsonIgnore]
        [Required(ErrorMessage = "O campo senha é obrigatório!")]
        [Display(Name = "Senha")]
        public string? Password { get; set; }

        public byte[]? HashPassword { get; set; }

        [Display(Name = "E-mail")]
        public string? Email { get; set; }
        [Display(Name = "Nome Completo")]
        public string? FullName { get; set; }

        public bool Ativo { get; set; }
    }
}