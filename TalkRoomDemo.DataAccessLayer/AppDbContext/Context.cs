using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.DataAccessLayer.AppDbContext
{
    public class Context : IdentityDbContext<AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server =UMIT\\SQLEXPRESS;initial catalog=TalkRoomDemoDB;integrated Security=true;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // === ServerUser için bileşik birincil anahtar ===
            builder.Entity<ServerUser>()
                .HasKey(su => new { su.ServerId, su.UserId });

            builder.Entity<ServerUser>()
                .HasOne(su => su.Server)
                .WithMany(s => s.ServerUsers)
                .HasForeignKey(su => su.ServerId)  
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ServerUser>()
                .HasOne(su => su.AppUser)
                .WithMany()
                .HasForeignKey(su => su.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // === FriendsRequest için ilişkiler ===
            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.SenderUser)
                .WithMany()
                .HasForeignKey(fr => fr.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.ReceiverUser)
                .WithMany()
                .HasForeignKey(fr => fr.ReceiverUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                 .HasOne(m => m.SenderUser)
                 .WithMany()
                 .HasForeignKey(m => m.SenderUserId)
                 .OnDelete(DeleteBehavior.Restrict); // <-- BURADA CASCADE YOK

            builder.Entity<Message>()
                .HasOne(m => m.ReceiverUser)
                .WithMany()
                .HasForeignKey(m => m.ReceiverUserId)
                .OnDelete(DeleteBehavior.Restrict); // <-- BURADA DA YOK

            // === Sunucu içi mesajlar (ServerMessage) ===

            

            builder.Entity<ServerMessage>()
                .HasOne(sm => sm.Server)
                .WithMany(s => s.Messages)
                .HasForeignKey(sm => sm.ServerId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade yerine Restrict

            builder.Entity<ServerMessage>()
                .HasOne(sm => sm.SenderUser)
                .WithMany()
                .HasForeignKey(sm => sm.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade yerine Restrict

            // === Arkadaşlık ilişkileri (Friendship) ===


            builder.Entity<Friends>()
                .HasKey(f => f.Id);

            builder.Entity<Friends>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friends>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);
        
        }

        public DbSet<Message> Messages { get; set; } // Kişisel mesajlar (birebir kullanıcı mesajlaşması)
        public DbSet<Server> Servers { get; set; }   // Sunucular (Discord'daki sunucular gibi)
        public DbSet<ServerMessage> ServerMessages { get; set; } // Sunucu içi mesajlar (kanal içi sohbetler)
        public DbSet<ServerUser> ServerUsers { get; set; } // Sunuculara üye kullanıcılar (hangi kullanıcı hangi sunucuda)
        public DbSet<FriendRequest> FriendRequests { get; set; } // Arkadaşlık istekleri (kullanıcılar arası arkadaşlık talepleri)
        public DbSet<Friends> Friend { get; set; } // Arkadaşlık ilişkileri (kullanıcılar arası arkadaşlık ilişkileri)

    }
}
 