using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;
using DemoGit.Infrastructure.Context.Interfaces;

namespace DemoGit.Infrastructure.Context.Repositories
{
    public class CompraEfetivadaRepository : Repository<CompraEfetivada>, ICompraEfetivadaRepository
    {
        public CompraEfetivadaRepository(DatabaseContext context)
        : base(context)
        {

        }
    }
}