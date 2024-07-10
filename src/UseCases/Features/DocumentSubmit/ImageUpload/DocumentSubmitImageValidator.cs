namespace UseCases.Features;

public class DocumentSubmitImageValidator: AbstractValidator<DocumentImageDto>
{
    public DocumentSubmitImageValidator()
    {
        RuleFor(d=> d.ProNumber)
            .NotNull().GreaterThan(0);
        
        RuleFor(d=> d.LegType)
            .NotNull().NotEmpty()
            .MinimumLength(6).MaximumLength(15);

        RuleFor(d=> d.ImagesInBase64)
            .NotNull().NotEmpty();
    }
    
}