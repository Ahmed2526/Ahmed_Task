using Ahmed_Task.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ahmed_Task.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var password = "Admin@123";

        var adminUser = new ApplicationUser
        {
            Id = "7c0d1b7a-3b8f-4c7c-8e2a-3b3b3e0c8f5d",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@admin.test",
            NormalizedEmail = "ADMIN@ADMIN.TEST",
            EmailConfirmed = true,
            SecurityStamp = "5b8a8bb5-0a44-4d30-9d18-8d0d6b2a9c2a",
            ConcurrencyStamp = "1e8aa2a5-6f6e-4f9f-9b31-6c4e6c1a8f4f",
            PasswordHash = "AQAAAAIAAYagAAAAEBnyKXu3Y3Gi3XA25xx6ZKdZPoQl6YFtZrJva8U7EFX8WysYVFR8283XucvWa+4TnA=="
        };

        var receptionistUser = new ApplicationUser
        {
            Id = "7c0d1b7a-3a8f-4d7c-4s2a-3b3b3e0c8f5d",
            UserName = "receptionist",
            NormalizedUserName = "RECEPTIONIST",
            Email = "receptionist@receptionist.test",
            NormalizedEmail = "RECEPTIONIST@RECEPTIONIST.TEST",
            EmailConfirmed = true,
            SecurityStamp = "5b8a8ab5-0a34-4d40-9d18-8d2d6b2d9c2a",
            ConcurrencyStamp = "1e8as2a5-6f2e-4f9f-9a31-6c4e6d1a8a4f",
            PasswordHash = "AQAAAAIAAYagAAAAEBnyKXu3Y3Gi3XA25xx6ZKdZPoQl6YFtZrJva8U7EFX8WysYVFR8283XucvWa+4TnA=="
        };

        builder.HasData(adminUser, receptionistUser);
    }
}
