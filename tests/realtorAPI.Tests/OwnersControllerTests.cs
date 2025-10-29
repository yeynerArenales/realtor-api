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
    public class OwnersControllerTests
    {
        [Test]
        public async Task GetOwners_ShouldReturnOk_WithApiResponseAndData()
        {
            var serviceMock = new Mock<IOwnerService>(MockBehavior.Strict);
            serviceMock.Setup(s => s.GetOwnersAsync(1, 10))
                       .ReturnsAsync(new List<OwnerResponseDto> { new OwnerResponseDto { Id = "1", Name = "A" } });

            var controller = new OwnersController(serviceMock.Object);

            var result = await controller.GetOwners(page: 1, pageSize: 10);

            var ok = result.Result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            var body = ok!.Value as ApiResponse<List<OwnerResponseDto>>;
            Assert.That(body, Is.Not.Null);
            Assert.That(body!.Succeeded, Is.True);
            Assert.That(body.Data, Is.Not.Null);
            Assert.That(body.Data!.Count, Is.EqualTo(1));
        }
    }
}


