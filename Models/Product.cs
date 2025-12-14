using System;
using System.ComponentModel.DataAnnotations;

namespace MyMicroservice.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();  // UUID primary key

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}