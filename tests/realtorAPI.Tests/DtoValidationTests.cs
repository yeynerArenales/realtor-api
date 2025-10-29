using NUnit.Framework;
using realtorAPI.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace realtorAPI.Tests
{
    public class DtoValidationTests
    {
        [Test]
        public void PropertyCreateDto_ShouldFail_WhenRequiredFieldsMissing()
        {
            var dto = new PropertyCreateDto();
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, validateAllProperties: true);

            Assert.That(isValid, Is.False);
            Assert.That(results.Count, Is.GreaterThan(0));
        }

        [Test]
        public void OwnerCreateDto_ShouldPass_WithValidData()
        {
            var dto = new OwnerCreateDto
            {
                Name = "John Doe",
                Address = "123 Main St"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, validateAllProperties: true);

            Assert.That(isValid, Is.True);
            Assert.That(results, Is.Empty);
        }
    }
}


