using Ahmed_Task.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ahmed_Task.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "b3d4c7b0-7d9b-4b5d-9e54-3b29b6d7f2a1",
                Name = Roles.Admin,
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "f4d5a2b3-7c4f-4e32-8b3f-0ef2b7a0d9c1"
            },
            new IdentityRole
            {
                Id = "e5a6c5f2-1b4a-4f9d-9a83-2f29c11c8f3b",
                Name = Roles.User,
                NormalizedName = "USER",
                ConcurrencyStamp = "9a1c7b5e-2d3f-4a6b-91b8-3f2d1c0e7a6b"
            },
            new IdentityRole
            {
                Id = "e5a6c5f2-1s4a-4f9d-9b83-2f69c11a8f3b",
                Name = Roles.Receptionist,
                NormalizedName = "RECEPTIONIST",
                ConcurrencyStamp = "9a1c6b5e-2d3f-4b6b-91c8-3f2e1c1e5a6g"
            });
    }
}
