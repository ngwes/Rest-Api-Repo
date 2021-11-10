using FluentValidation;
using Rest_Api_Repo.Domain.Requests.V1;
using System.Linq;

namespace Rest_Api_Repo.Domain.Validators
{
    public class PostRequestValidator : AbstractValidator<PostRequest>
    {
        public PostRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Please provide a valid Name");

            RuleFor(x => x.NewTags)
                .NotNull()
                .ForEach(tag => tag.Matches("^#[a-zA-Z0-9 ]*$"))
                .WithMessage("Uncorrect Tags Format");

            RuleFor(x => x.ExistingTags.Select(x => x.ToString()).ToList())
                .NotNull()
                .ForEach(tag => tag.Matches("^#[a-zA-Z0-9 ]*$"))
                .WithMessage("Uncorrect existing tag use");
        }
    }
}
