namespace UseCases.Features.Login;

public class GetUserTokenValidation: AbstractValidator<LoginRequestDto>
{
    public GetUserTokenValidation()
    {
        RuleFor(r=> r.UserId.Value.Value)
        .NotEmpty()
        .MinimumLength(2)
        .MaximumLength(10)
        .WithMessage("Invalid {PropertyName}. Must be 2-10 characters long");

        RuleFor(r=> r.Password.Value)
        .NotEmpty()
        .MinimumLength(2)
        .MaximumLength(50)
        .WithMessage("Invalid {PropertyName}. Must be 2-50 characters long");
    }
}