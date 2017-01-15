using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" },
                new Product { ProductID = 4, Name = "P4" },
                new Product { ProductID = 5, Name = "P5" }
            });

            //Act
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            IEnumerable<Product> result = ((ProductsListViewModel) controller.List(null, 2).Model).Products;

            //Assert
            Product[] prodArray = result.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            //Arrange
            // - create the mock rep
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Action
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Apples" },
                new Product { ProductID = 2, Name = "P2", Category = "Apples" },
                new Product { ProductID = 3, Name = "P3", Category = "Plums" },
                new Product { ProductID = 4, Name = "P4", Category = "Oranges" }
            });

            NavController target = new NavController(mock.Object);

            //Act
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            //Assert
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Apples" },
                new Product { ProductID = 4, Name = "P2", Category = "Oranges" }
            });

            NavController target = new NavController(mock.Object);

            string categoryToSelect = "Apples";

            //Action
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            //Assert
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Action
            int result1 = ((ProductsListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int result2 = ((ProductsListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int result3 = ((ProductsListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resultNull = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            //Assert
            Assert.AreEqual(2, result1);
            Assert.AreEqual(2, result2);
            Assert.AreEqual(1, result3);
            Assert.AreEqual(5, resultNull);
        }
    }
}
