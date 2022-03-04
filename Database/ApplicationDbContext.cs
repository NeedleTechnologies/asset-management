using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asset_management.Entities;
using asset_management.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Database
{
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options) {

        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentFolder> DocumentFolders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
