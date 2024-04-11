using FluentValidation;
using OxuAzz.Dtos.NewDto;

namespace OxuAzz.Validations.News.News
{
    public class NewDtoValidation : AbstractValidator<NewPostDto>
    {
        public NewDtoValidation()
        {



            RuleFor(x => x.Title).NotEmpty().WithMessage("Title field cannot be empty.").NotNull().WithMessage("Title field cannot be null.").MaximumLength(100).WithMessage("Description must be at least 100 characters long.");
            RuleFor(x => x.Description).MaximumLength(2000).WithMessage("Description must be at least 2000 characters long.").NotEmpty().WithMessage("Description field cannot be empty.").NotNull().WithMessage("Description field cannot be null.");
            RuleFor(x => x.ImgUrl).NotNull().WithMessage("Image URL cannot be null.").NotEmpty().WithMessage("Image URL cannot be empty.");
            RuleFor(x => x.CategoryId).NotNull().WithMessage("Category ID cannot be null.").NotEqual(0).WithMessage("Category ID must be specified.");

        }
    }
}

