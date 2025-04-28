using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator :AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];
    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name).
            NotEmpty().WithMessage("Please provide a valid name")
            .Length(2, 50);

        RuleFor(dto => dto.Category)
        //.Must(category => validCategories.Contains(category))
        //.WithMessage("Invalid category. Please choose from the valid categories.");
        .Custom((value, context) =>
        {
            var isValidCategory = validCategories.Contains(value);
            if (!isValidCategory)
            {
                context.AddFailure("Category", "Invalid category. Please choose from the valid categories.");
            }
        });

        RuleFor(dto => dto.ContactEmail)
        .EmailAddress()
        .WithMessage("Please provide a valid email address");

        RuleFor(dto => dto.PostalCode)
        .Matches(@"^\d{2}-\d{3}$")
        .WithMessage("Please provide a valid postal code (XX-XXX).");
    }
}
