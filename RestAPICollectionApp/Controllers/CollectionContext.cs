using RestAPICollectionApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RestAPICollectionApp.Controllers
{
    public class CollectionContext : DbContext
    {
        public DbSet<CollectionModel> Collections { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}