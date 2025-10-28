using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace realtorAPI.Helpers
{
    public static class ModelStateExtensions
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            var errors = new List<string>();
            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    errors.Add($"{entry.Key}: {error.ErrorMessage}");
                }
            }
            return errors;
        }
    }
}

