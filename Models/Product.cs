﻿using System.ComponentModel.DataAnnotations;

namespace HerexamenEcommerce24.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
