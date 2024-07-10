namespace UseCases.Extensions;

public static class PropertyAccessorExtensionMethods
{ 
    public static object Get<T>(this T Object, string PropertyName) where T : class => Object.GetType().GetProperty(PropertyName) is var v && v is not null ? v.GetValue(Object) : null;
    public static Nullable<int> GetInt<T>(this T Object, string PropertyName) where T : class => Object.GetType().GetProperty(PropertyName) is var v && v is not null ? Convert.ToInt32(v.GetValue(Object)) : null;
    public static string GetString<T>(this T Object, string PropertyName) where T : class => Object.GetType().GetProperty(PropertyName) is var v && v is not null ? Convert.ToString(v.GetValue(Object)) : "";
    public static Nullable<double> GetDouble<T>(this T Object, string PropertyName) where T : class => Object.GetType().GetProperty(PropertyName) is var v && v is not null ? Convert.ToDouble(v.GetValue(Object)) : null;
    public static Nullable<decimal> GetDecimal<T>(this T Object, string PropertyName) where T : class => Object.GetType().GetProperty(PropertyName) is var v && v is not null ? Convert.ToDecimal( v.GetValue(Object)) : null;
    public static Nullable<bool> GetBool<T>(this T Object, string PropertyName) where T : class => Object.GetType().GetProperty(PropertyName) is var v && v is not null  ? Convert.ToBoolean(v.GetValue(Object)) : null;
    public static Nullable<DateTime> GetDate<T>(this T Object, string PropertyName) where T : class => Object.GetType().GetProperty(PropertyName).GetValue(Object) is var v && v is not null ? Convert.ToDateTime(v) : null;

    public static void SetValue<T>(this T Object, string PropertyName, object value) where T : class => Object.GetType().GetProperty(PropertyName).SetValue(Object, value);

}
