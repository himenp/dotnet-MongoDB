using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace dotnetfundaMongoDB.Models
{
    public sealed class MongoDBContext
    {
        public const string _databaseName = "blog";
        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;

        static MongoDBContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(_databaseName);
        }

        public IMongoClient Client
        {
            get { return _client; }
        }

        public IMongoCollection<Post> Posts
        {
            get { return _database.GetCollection<Post>("posts"); }
        }
        public static MongoDBContext Instance
        {
            get
            {
                return DBContext.instance;
            }
        }
        private class DBContext
        {
            static DBContext()
            {
            }
            internal static readonly MongoDBContext instance = new MongoDBContext();
        }
    }
}