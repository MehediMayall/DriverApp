namespace UseCases.Extensions;

public static class ValueObjectConversionMethods
{
    // INT
    public static CompanyID GetCompanyID(this int value) => new CompanyID((NonNegative) value);
    public static CompanyID GetCompanyID(this int? value) => GetCompanyID(value.GetValueOrDefault());
    public static CompanyID GetCompanyID(this string value) => GetCompanyID(int.Parse(value.Trim()));
    public static DriverID GetDriverID(this int value) => new DriverID((NonNegative) value);
    public static DriverID GetDriverID(this int? value) => GetDriverID(value.GetValueOrDefault());
    public static ProNumber GetProNumber(this int value) => new ProNumber((NonNegative) value);
    public static ProNumber GetProNumber(this int? value) => GetProNumber(value.GetValueOrDefault());
    public static OrderLogID GetOrderLogID(this int value) => new OrderLogID((NonNegative) value);
    public static OrderLogID GetOrderLogID(this int? value) => GetOrderLogID(value.GetValueOrDefault());
    public static DocumentID GetDocumentID(this int value) => new DocumentID((NonNegative) value);
    public static DocumentID GetDocumentID(this int? value) => GetDocumentID(value.GetValueOrDefault());


    // STRING
    public static UserID GetUserID(this string value) => new UserID((NonEmptyString)value);
    public static LoadType GetLoadType(this string value) => new LoadType(value);
    public static LegType GetLegType(this string value) => new LegType((NonEmptyString)value);
}
