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
    public class CartProductConfigurations : IEntityTypeConfiguration<CartProduct>
    {
        public void Configure(EntityTypeBuilder<CartProduct> builder)
        {
            builder.HasKey(CP => new { CP.CartId, CP.ProductId });

            /*
             * One-to-Many from Cart to CartProducts:
             *      One Cart can have multiple products, represented by multiple entries in the 
             *      CartProduct table.
             * One-to-Many from Product to CartProducts:
             *      One Product can be in multiple carts, represented by multiple entries in the 
             *      CartProduct table.
             */
            builder.HasOne(CP => CP.Cart)
                   .WithMany(C => C.CartProducts)
                   .HasForeignKey(CP => CP.CartId)
                   .HasPrincipalKey(C => C.CustomerId)     //as in Cart I don't have key named CartId 
                   .OnDelete(DeleteBehavior.Cascade);

            //It will applied by default
            builder.HasOne(CP => CP.Product)
                   .WithMany(P => P.CartProducts)
                   .HasForeignKey(P => P.ProductId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
