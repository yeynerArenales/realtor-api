using Microsoft.AspNetCore.Mvc.ModelBinding;
using NUnit.Framework;
using realtorAPI.Helpers;

namespace realtorAPI.Tests
{
    public class ModelStateExtensionsTests
    {
        [Test]
        public void GetErrors_ShouldReturnAllModelErrors()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("Name", "Required");
            modelState.AddModelError("Price", "Must be positive");

            var errors = modelState.GetErrors();

            Assert.That(errors, Has.Count.EqualTo(2));
            Assert.That(errors[0], Does.Contain("Name"));
            Assert.That(errors[0], Does.Contain("Required"));
            Assert.That(errors[1], Does.Contain("Price"));
        }
    }
}


