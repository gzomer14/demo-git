using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DemoGit.Domain.Entities
{
    public class CompraEfetivada
    {
        public CompraEfetivada()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        [BsonId]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string? ProdutoId { get; set; }

        public int QuantidadeCompra { get; set; }

        public double ValorTotal { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string? UsuarioId { get; set; }
    }
}