using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Linq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class IntegrationTests
    {
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<ProductService> _localizer;
        private readonly string myServer = "Server=localhost\\SQLEXPRESS;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true";

        [Fact]
        public async Task SaveNewProduct()
        {
            //Arrange        
            var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(myServer).Options;
            P3Referential ctx = new(options, _configuration);

            LanguageService languageService = new();
            Cart cart = new();
            ProductRepository productRepository = new(ctx);
            OrderRepository orderRepository = new(ctx);
            ProductService productService = new(cart, productRepository, orderRepository, _localizer);
            ProductController productController = new(productService, languageService);

            ProductViewModel productViewModel = new()
            {
                Name = "Produit test à ajouter",
                Description = "",
                Details = "",
                Stock = "1",
                Price = "10"
            };

            int count = await ctx.Product.CountAsync();

            //Act
            productController.Create(productViewModel);

            //Assert
            Assert.Equal(count + 1, ctx.Product.Count());
            var product = await ctx.Product.Where(item => item.Name == "Produit test à ajouter").FirstOrDefaultAsync();
            Assert.NotNull(product);

            //End
            ctx.Product.Remove(product);
            await ctx.SaveChangesAsync();
        }
        [Fact]
        public async Task DeleteProduct()
        {
            //Arrange    
            var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(myServer).Options;
            P3Referential ctx = new(options, _configuration);

            LanguageService languageService = new();
            Cart cart = new();
            ProductRepository productRepository = new(ctx);
            OrderRepository orderRepository = new(ctx);
            ProductService productService = new(cart, productRepository, orderRepository, _localizer);
            ProductController productController = new(productService, languageService);

            ProductViewModel productViewModel = new()
            {
                Name = "Produit test à supprimer",
                Description = "",
                Details = "",
                Stock = "1",
                Price = "10"
            };

            int count = await ctx.Product.CountAsync();
            productController.Create(productViewModel);
            var product = await ctx.Product.Where(item => item.Name == "Produit test à supprimer").FirstOrDefaultAsync();
            //Act
            productController.DeleteProduct(product.Id);

            //Assert
            Assert.Equal(count, ctx.Product.Count());
            var searchProductAgain = await ctx.Product.Where(item => item.Name == "Produit test à supprimer").FirstOrDefaultAsync();
            Assert.Null(searchProductAgain);
        }
    }
}
