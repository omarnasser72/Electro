using Electro.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Repository.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.Category)
                   .WithMany(C => C.Products)
                   .HasForeignKey(P => P.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(P => P.Brand)
                   .WithMany(C => C.Products)
                   .HasForeignKey(P => P.BrandId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
