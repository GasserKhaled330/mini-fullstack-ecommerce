namespace Ecommerce.Api.Filters;

public class CustomValidationResultFactory : IFluentValidationAutoValidationResultFactory
{
  public Task<IActionResult?> CreateActionResult(
    ActionExecutingContext context,
    ValidationProblemDetails validationProblemDetails,
     IDictionary<IValidationContext, ValidationResult> validationResults)
  {
    var errorMessages = validationProblemDetails?.Errors
            .SelectMany(x => x.Value)
            .ToList() ?? [];

    var errorMessage = string.Join(" | ", errorMessages);

    var problemDetails = new ProblemDetails
    {
      Status = StatusCodes.Status400BadRequest,
      Title = "Validation Error",
      Detail = errorMessage,
      Instance = context.HttpContext.Request.Path
    };

    return Task.FromResult<IActionResult?>(new BadRequestObjectResult(problemDetails));
  }
}
