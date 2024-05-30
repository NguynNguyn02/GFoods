    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFoods.Models
{
    public class Product : CommonAbstract
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ProductCode { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string Detail {  get; set; }
        public int Quantity { get; set; }
        [ValidateNever]
        public string ImageUrl {  get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceSale { get; set; }

        [StringLength(250)]
        public string ?Alias { get; set; }
        public int ViewCount { get; set; }
        public bool IsHome { get; set; }
        public bool IsFeature { get; set; }
        public bool IsSale { get; set; }
        public bool IsHot { get; set; }
        
        [StringLength(250)]

        public string SeoTitle { get; set; }
        [StringLength(500)]

        public string SeoDescription { get; set; }
        [StringLength(250)]

        public string SeoKeywords { get; set; }
        public int CategoryProductId { get; set; }

        [ForeignKey("CategoryProductId")]
        [ValidateNever]
        public CategoryProduct CategoryProduct { get; set; }

    }
}
