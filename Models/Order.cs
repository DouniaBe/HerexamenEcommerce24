using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HerexamenEcommerce24.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
