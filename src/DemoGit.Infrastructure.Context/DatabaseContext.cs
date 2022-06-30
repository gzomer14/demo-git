using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DemoGit.Infrastructure.Context;
public class DatabaseContext
{
    private readonly MongoClient client;

    public DatabaseContext(IOptions<ConnectionStrings> conn)
    {
        client = new MongoClient(conn.Value.MongoDB);
    }

    public IMongoDatabase GetDatabase(string database)
        => client.GetDatabase(database);
}