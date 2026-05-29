using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ahmed_Task.Configuration;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string>
            {
                UserId = "7c0d1b7a-3b8f-4c7c-8e2a-3b3b3e0c8f5d",
                RoleId = "b3d4c7b0-7d9b-4b5d-9e54-3b29b6d7f2a1"
            },
            new IdentityUserRole<string>
            {
                UserId = "7c0d1b7a-3b8f-4c7c-8e2a-3b3b3e0c8f5d",
                RoleId = "e5a6c5f2-1b4a-4f9d-9a83-2f29c11c8f3b"
            },
            new IdentityUserRole<string>
            {
                UserId = "7c0d1b7a-3a8f-4d7c-4s2a-3b3b3e0c8f5d",
                RoleId = "e5a6c5f2-1s4a-4f9d-9b83-2f69c11a8f3b"
            });
    }
}
