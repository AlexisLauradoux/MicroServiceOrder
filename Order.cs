using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroServiceOrder
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
  
        public bool Done { get; set; }

        public string Address { get; set; }

        public virtual ICollection<ItemOrder>  Items { get; set; }

    }
}
