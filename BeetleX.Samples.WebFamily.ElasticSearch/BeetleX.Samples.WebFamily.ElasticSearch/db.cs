using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Samples.WebFamily.ElasticSearch
{
    public class BlogDBContext : DbContext
    {
        public BlogDBContext()
        {
        }

        public BlogDBContext(DbContextOptions<BlogDBContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Filename=BeetlexBlog.db");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }



        public DbSet<Post> Posts { get; set; }


    }

    [Table("Posts")]
    public class Post
    {
        [Key]
        public string ID { get; set; }

        public string Title { get; set; }

        public string Project { get; set; }

        public string Tag { get; set; }

        public string SourceUrl { get; set; }

        public string Content { get; set; }

        public string Digest { get; set; }

        public bool Enabled { get; set; }

        public string User { get; set; }

        public string NickName { get; set; }

        public DateTime CreateTime { get; set; }

        public int Level { get; set; }
    }
}
