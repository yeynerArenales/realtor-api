using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using realtorAPI.Controllers;
using realtorAPI.Services;
using realtorAPI.DTOs;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace realtorAPI.Tests
{
    public class PropertiesControllerTests
    {
        [Test]
        public async Task GetProperties_ShouldReturnOk_WithApiResponseAndData()
        {
            var serviceMock = new Mock<IPropertyService>(MockBehavior.Strict);
            serviceMock.Setup(s => s.GetPropertiesAsync(It.IsAny<PropertyFilterDto>()))
                       .ReturnsAsync(new List<PropertyResponseDto> { new PropertyResponseDto { Id = "1", Name = "X" } });

            var controller = new PropertiesController(serviceMock.Object);

            var result = await controller.GetProperties(new PropertyFilterDto());

            var ok = result.Result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            var body = ok!.Value as ApiResponse<List<PropertyResponseDto>>;
            Assert.That(body, Is.Not.Null);
            Assert.That(body!.Succeeded, Is.True);
            Assert.That(body.Data, Is.Not.Null);
            Assert.That(body.Data!.Count, Is.EqualTo(1));
        }
    }
}


