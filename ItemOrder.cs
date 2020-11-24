using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServiceOrder
{
    public class ItemOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }
        public int Quantity { get; set; }

    }

    public class Product
    {
        public string Id { get; set; }
        public string Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }

        public int Quantity { get; set; }

        public static implicit operator Product(EntrepotService.Product productEntrepot)
        {
            Product product = new();
            product.Picture = productEntrepot.Picture;
            product.Id = productEntrepot.Id;
            product.Name = productEntrepot.Name;
            product.Quantity = productEntrepot.Quantity;
            product.Type = productEntrepot.Type;
            product.Description = productEntrepot.Description;

            return product;
        }
    }
}
