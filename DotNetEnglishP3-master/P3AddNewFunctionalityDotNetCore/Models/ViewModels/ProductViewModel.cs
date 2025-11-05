using Microsoft.AspNetCore.Mvc.ModelBinding;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Resources.Models.Services;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "MissingName")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required(ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "MissingStock")]
        [RegularExpression(@"^\s*-?\d+\s*$", ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "StockNotAnInteger")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "StockNotGreaterThanZero")]
        public string Stock { get; set; }

        [Required(ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "MissingPrice")]
        [RegularExpression(@"^\s*-?\d+([.]\d+)?\s*$", ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "PriceNotANumber")]
        [Range(0.01, double.MaxValue, ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "PriceNotGreaterThanZero")]
        public string Price { get; set; }
    }
}