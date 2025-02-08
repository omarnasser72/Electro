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
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(O => new { O.CustomerId, O.DateTime });

            builder.HasOne(O => O.Customer)
                   .WithMany(C => C.Orders)
                   .HasForeignKey(O => O.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(C => C.OrderProducts)
                   .WithOne(OP => OP.Order)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
