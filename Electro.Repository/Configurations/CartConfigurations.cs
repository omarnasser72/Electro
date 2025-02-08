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
    public class CartConfigurations : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {

            builder.HasOne(C => C.Customer)
                   .WithOne(Cust => Cust.Cart)
                   .HasForeignKey<Cart>(C => C.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(C => C.CartProducts)
                   .WithOne();

            builder.HasKey(C => C.CustomerId);
        }
    }
}
