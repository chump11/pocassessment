using FluentValidation;

namespace POCAssessment.Application.Products;

public class ProductFilterQueryValidation : AbstractValidator<ProductFilterQuery>
{
    public ProductFilterQueryValidation()
    {

        RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0)
               .WithMessage("Min Price is to higher than zero");

        RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(0)
               .WithMessage("Max Price is to higher than zero");

        RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(x => x.MinPrice)
               .WithMessage("Max Price is to higher than min price");

        RuleFor(x => x.Size).Matches(@"^[0-9a-zA-Z ]+$")
            .WithMessage("Numbers and letters only please.");

        RuleFor(x => x.Highlight).Matches(@"^[0-9a-zA-Z ,]+$")
            .WithMessage("Numbers and letters only please.");
    }
}
