using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using Xunit;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {

        private static List<ValidationResult> GetModelValidationErrors(ProductViewModel model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);

            Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);

            return validationResults;
        }


        [Fact]
        public void Product_Is_Valid()
        {
            //Arrange
            var model = new ProductViewModel
            {
                Name = "name",
                Stock = "15",
                Price = "10"
            };

            //Act
            var errors = GetModelValidationErrors(model);

            //Assert
            Assert.Empty(errors);
        }

        [Fact]
        public void Product_Is_Unvalid_EmptySlots()
        {
            //Arrange
            var model = new ProductViewModel
            {
                Name = "",
                Stock = "",
                Price = ""
            };

            //Act
            var errors = GetModelValidationErrors(model);

            //Assert
            Assert.Equal(3, errors.Count);
            Assert.Contains(errors, e => e.ErrorMessage == "Veuillez saisir un nom");
            Assert.Contains(errors, e => e.ErrorMessage == "Veuillez saisir un prix");
            Assert.Contains(errors, e => e.ErrorMessage == "Veuillez saisir un stock");
        }

        [Fact]
        public void Product_Is_Unvalid_NotNumbers()
        {
            //Arrange
            var model = new ProductViewModel
            {
                Name = "name",
                Stock = "stock",
                Price = "price"
            };

            //Act
            var errors = GetModelValidationErrors(model);

            //Assert
            Assert.Equal(4, errors.Count);
            Assert.Contains(errors, e => e.ErrorMessage == "La valeur saisie pour le prix doit être un nombre");
            Assert.Contains(errors, e => e.ErrorMessage == "La prix doit être supérieur à zéro");
            Assert.Contains(errors, e => e.ErrorMessage == "La valeur saisie pour le stock doit être un entier");
            Assert.Contains(errors, e => e.ErrorMessage == "La stock doit être supérieure à zéro");
        }

        [Fact]
        public void Product_Is_Unvalid_NumbersWithComma()
        {
            //Arrange
            var model = new ProductViewModel
            {
                Name = "name",
                Stock = "15,5",
                Price = "10,3"
            };

            //Act
            var errors = GetModelValidationErrors(model);

            //Assert
            Assert.Equal(2, errors.Count);
            Assert.Contains(errors, e => e.ErrorMessage == "La valeur saisie pour le stock doit être un entier");
            Assert.Contains(errors, e => e.ErrorMessage == "La stock doit être supérieure à zéro");
        }

        [Fact]
        public void Product_Is_Unvalid_NumbersWithPoint()
        {
            //Arrange
            var model = new ProductViewModel
            {
                Name = "name",
                Stock = "24.5",
                Price = "6.3"
            };

            //Act
            var errors = GetModelValidationErrors(model);

            //Assert
            Assert.Equal(2, errors.Count);
            Assert.Contains(errors, e => e.ErrorMessage == "La valeur saisie pour le stock doit être un entier");
            Assert.Contains(errors, e => e.ErrorMessage == "La stock doit être supérieure à zéro");
        }
        [Fact]
        public void Product_Is_Unvalid_NegativeNumbers()
        {
            //Arrange
            var model = new ProductViewModel
            {
                Name = "name",
                Stock = "-15",
                Price = "-3"
            };

            //Act
            var errors = GetModelValidationErrors(model);

            //Assert
            Assert.Equal(2, errors.Count);
            Assert.Contains(errors, e => e.ErrorMessage == "La prix doit être supérieur à zéro");
            Assert.Contains(errors, e => e.ErrorMessage == "La stock doit être supérieure à zéro");
        }

        [Fact]
        public void Product_Is_Unvalid_NumberZero()
        {
            //Arrange
            var model = new ProductViewModel
            {
                Name = "name",
                Stock = "0",
                Price = "0"
            };

            //Act
            var errors = GetModelValidationErrors(model);

            //Assert
            Assert.Equal(2, errors.Count);
            Assert.Contains(errors, e => e.ErrorMessage == "La prix doit être supérieur à zéro");
            Assert.Contains(errors, e => e.ErrorMessage == "La stock doit être supérieure à zéro");
        }

        [Fact]
        public void Product_Is_Valid_NumberStartsWithZero()
        {
            //Arrange
            var model = new ProductViewModel
            {
                Name = "name",
                Stock = "015",
                Price = "0010"
            };

            //Act
            var errors = GetModelValidationErrors(model);

            //Assert
            Assert.Empty(errors);
        }

        [Fact]
        public void Product_Is_Valid_WithBlankSpaces()
        {
            //Arrange
            var model = new ProductViewModel
            {
                Name = "   name   ",
                Stock = "  15  ",
                Price = " 10 "
            };

            //Act
            var errors = GetModelValidationErrors(model);

            //Assert
            Assert.Empty(errors);
        }




    }
}