namespace Ecommerce.Api.Startup;

public static class ValidatorsConfiguration
{
	public static void AddValidators(this IServiceCollection services)
	{
		services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
		services.AddFluentValidationAutoValidation(configuration =>
		{
			configuration.OverrideDefaultResultFactoryWith<CustomValidationResultFactory>();
		});
	}
}
