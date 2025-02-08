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
    public class OrderProductConfigurations : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasKey(OP => new { OP.CustomerId, OP.Date, OP.ProductId });

            builder.HasOne(OP => OP.Product)
                   .WithMany(P => P.OrderProducts)
                   .HasForeignKey(OP => OP.ProductId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(OP => OP.Order)
                   .WithMany(O => O.OrderProducts)
                   .HasForeignKey(OP => new { OP.CustomerId, OP.Date })
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
