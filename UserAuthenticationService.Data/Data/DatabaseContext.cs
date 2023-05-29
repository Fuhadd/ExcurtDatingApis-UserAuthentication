using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Data.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser, IdentityRole<int>, int>
    {
        public DatabaseContext(DbContextOptions options) :base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserMatch>()
                .HasOne(x => x.SentBy)
                .WithMany(x => x.MatchesSent)
                .HasForeignKey(x => x.SentById)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<UserMatch>()
                .HasOne(x => x.SentTo)
                .WithMany(x => x.MatchesReceieved)
                .HasForeignKey(x => x.SentToId)
                .OnDelete(DeleteBehavior.ClientSetNull);

           // builder.HasDbFunction(typeof(ValidationFunctions).GetMethod(nameof(ValidationFunctions.SplitByDash)))
   // .HasName("SplitByDash");


        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<UserMatch> UserMatches { get; set; }
        public DbSet<Message> Messages { get; set; }

    }
}
