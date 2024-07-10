namespace Domain.Exceptions;

public static class LegTypeError<LegType> where LegType : class
{     
    public static Error<LegType> InvalidLegType(string LegType)
    {
        return new($"Invalid Leg Type: {LegType}","");
    }
}