using FluentValidation;
using Rest_Api_Repo.Contracts.V1.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Validators
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

            RuleFor(x => x.ExistingTags.Select(x=>x.ToString()).ToList())
                .NotNull()
                .ForEach(tag => tag.Matches("^#[a-zA-Z0-9 ]*$"))
                .WithMessage("Uncorrect existing tag use");
        }
    }
}
