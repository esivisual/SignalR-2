using Microsoft.EntityFrameworkCore;
using Site.Models.Entites;

namespace Site.Context
{
    public class DataBaseContext:DbContext
    {
        public DataBaseContext(DbContextOptions options) :base(options)
        {
        }
        public DbSet<ChatRoom>? ChatRooms { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<ChatMessage>?  ChatMessages{ get; set; }

    }
}
