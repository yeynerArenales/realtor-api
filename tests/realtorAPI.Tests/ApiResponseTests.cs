using NUnit.Framework;
using realtorAPI.DTOs;

namespace realtorAPI.Tests
{
    public class ApiResponseTests
    {
        [Test]
        public void Success_ShouldSetSucceededTrue_AndMessageAndData()
        {
            var data = new { Value = 1 };

            var response = ApiResponse<object>.Success(data, "ok");

            Assert.That(response.Succeeded, Is.True);
            Assert.That(response.Message, Is.EqualTo("ok"));
            Assert.That(response.Errors, Is.Empty);
            Assert.That(response.Data, Is.EqualTo(data));
        }

        [Test]
        public void Error_WithSingleMessage_ShouldSetSucceededFalse_AndErrorList()
        {
            var response = ApiResponse<object>.Error("bad", "detail");

            Assert.That(response.Succeeded, Is.False);
            Assert.That(response.Message, Is.EqualTo("bad"));
            Assert.That(response.Errors, Has.Count.EqualTo(1));
            Assert.That(response.Errors[0], Is.EqualTo("detail"));
            Assert.That(response.Data, Is.Null);
        }
    }
}


