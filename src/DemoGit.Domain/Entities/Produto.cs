using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoGit.Domain.Entities
{
    public class Produto
    {
        public string Id { get; set; } = $"produto:{Guid.NewGuid().ToString()}";
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}