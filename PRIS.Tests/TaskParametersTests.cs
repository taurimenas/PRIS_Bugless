using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PRIS.Core.Library.Entities;
using PRIS.Web.Controllers;
using PRIS.Web.Models;
using PRIS.Web.Storage;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;
using System.Web;

namespace PRIS.Tests
{
    public class TaskParametersTests
    {
        [Fact]
        public async Task Edit_ReturnsAViewResult_WithOneTaskParameter()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            var mockLogger = new Mock<ILogger<ExamsController>>();
            mockRepo.Setup(repo => repo.FindByIdAsync<Exam>(2))
                .ReturnsAsync(GetTestData());
            var controller = new TaskParametersController(mockRepo.Object, mockLogger.Object);
            // Act
            var result = await controller.Edit(2);
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SetTaskParameterModel>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Id);
            Assert.NotEqual(0, model.Id);
            Assert.NotNull(model);
        }

        private Exam GetTestData()
        {
            return new Exam
            {
                Id = 2,
                Created = new DateTime(2016, 7, 2),
                Date = new DateTime(2020, 1, 2),
                Tasks = "[1, 1, 1, 1, 1, 1, 1, 1, 2, 4]"
            };
        }
    }
}
