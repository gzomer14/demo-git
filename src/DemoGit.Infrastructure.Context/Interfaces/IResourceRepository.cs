using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;

namespace DemoGit.Infrastructure.Context.Interfaces
{
    public interface IResourceRepository : IRepository<Resource>
    {
        List<Resource> GetImages();
        Resource SelectByFileName(string fileName);
    }
}