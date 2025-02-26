using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sti3_api.Domain.Entities;

namespace sti3_api.Infrastructure.Persistence.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(c => c.ClientId);
            
            builder.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(c => c.Cpf)
                .IsRequired();
        }
    }
}