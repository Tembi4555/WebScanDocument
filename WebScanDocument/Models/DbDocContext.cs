using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebScanDocument.Models
{
    public class DbDocContext : DbContext
    {
        public DbSet<Worker> Workers { get; set; }
        public DbSet<TypeOfDocument> TypeOfDocuments { get; set; }
        public DbSet<ListOfDocument> ListOfDocuments { get; set; }
        public DbSet<RegisterOfDocPage> RegisterOfDocPages { get; set; }
    }
}