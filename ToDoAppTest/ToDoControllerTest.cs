using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Controllers;
using ToDoApp.Data;
using ToDoApp.Models;
using Xunit;

namespace ToDoAppTest
{
    public class ToDoControllerTest
    {
        private readonly Mock<IToDoRepository> repositoryStub = new Mock<IToDoRepository>();

        private IEnumerable<ToDo> CreateTODOs()
        {

            var res = new List<ToDo>();
            for (int i = 1; i <= 5; i++)
            {
                var tmp = new ToDo()
                {
                    Id = i,
                    Description = "Data" + i,
                    IsDone = false
                };

                res.Add(tmp);
            }

            return res;
        }
        [Fact]
        public void Test_Get_WithExpectedItems()
        {
            var expectedTODOItems = CreateTODOs();

            repositoryStub.Setup(repo => repo.Get()).ReturnsAsync(expectedTODOItems);

            var controller = new ToDoController(repositoryStub.Object);

            var result = (OkObjectResult)controller.Get().Result.Result;

            Assert.Equal(expectedTODOItems, result.Value);
        }

        [Fact]
        public void Test_Search_Ok()
        {
            var expectedTODOItems = CreateTODOs();

            repositoryStub.Setup(repo => repo.Search("Data")).ReturnsAsync(expectedTODOItems);

            var controller = new ToDoController(repositoryStub.Object);

            var result = (OkObjectResult)controller.Search("Data").Result.Result;

            Assert.Equal(expectedTODOItems, result.Value);
        }
    }
}
