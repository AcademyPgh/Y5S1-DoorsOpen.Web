using BlogThing.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogThing.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<SecurityUser> User { get; set; }
        public DbSet<SecurityGroup> Groups { get; set; }
    }
}
