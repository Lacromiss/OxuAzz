using FluentValidation;
using OxuAzz.Dtos.CategoryDto;
using OxuAzz.Models;

namespace OxuAzz.Validations.Categories
{
    public class CategoryPostValidation:AbstractValidator<CategoryPostDto>
    {
        public CategoryPostValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name field cannot be empty.").NotNull().WithMessage("Name field cannot be null.").MaximumLength(100).WithMessage("Name must be at least 100 characters long.");

        }
    }
}
