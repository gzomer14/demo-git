using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoGit.Infrastructure.Context.Repositories
{
    public class ProdutoRepository
    {
        private readonly DatabaseContext _context;

        public ProdutoRepository(DatabaseContext context)
        {
            _context = context;
        }
    }
}