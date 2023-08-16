using FluentValidation;
using FluentValidation.Results;

namespace FTWRK.Application.Common.Models
{
    public abstract class CustomAbstractValidator<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var validationResult = base.Validate(context);

            if (!validationResult.IsValid)
            {
                try
                {
                    RaiseValidationException(context, validationResult);
                }
                catch (Exception ex) { }
            }

            return validationResult;
        }
    }
}
