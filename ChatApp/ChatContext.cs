using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp;

public class ChatContext :DbContext
{
    public DbSet<User> Users{ get; set; }
    public DbSet<Message> Messages{ get; set; }
    public ChatContext() : base()
    {
       Database.EnsureCreated();


    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("data source=./chat.db");
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(x =>
        {
            x.HasMany(x=>x.Messages).WithOne(x=>x.Author).HasForeignKey(x=>x.AuthorId);
        });
        base.OnModelCreating(modelBuilder);
    }
}
