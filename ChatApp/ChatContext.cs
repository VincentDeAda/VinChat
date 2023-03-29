using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp;

public class ChatContext : DbContext
{
    public DbSet<Identity> Identities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageEdit> EditHistory { get; set; } = null!;
    public DbSet<RemovedMessage> RemovedMessages { get; set; } = null!;
    public DbSet<EmailConfirmation> EmailConfirmations { get; set; } = null!;
    public ChatContext() : base() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("data source=./chat.db");
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
     
        modelBuilder.Entity<User>(x =>
        {
            x.HasMany(x => x.Messages).WithOne(x => x.Author).HasForeignKey(x => x.AuthorId);
            x.HasOne(x => x.Identity).WithOne(x => x.User).HasForeignKey<User>(x => x.IdentityId);
        });
        modelBuilder.Entity<Message>(x =>
        {
            x.HasMany(x => x.EditHistroy).WithOne(x => x.Message).HasForeignKey(x => x.MessageId);
        });
        base.OnModelCreating(modelBuilder);
    }
}
