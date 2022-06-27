using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;

namespace DemoGit.Infrastructure.Context.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    List<Produto> SelectLikeDescription(string pesquisa);
}
