using System.Collections.ObjectModel;

namespace Domain.Constants;

public sealed class LegTypeList
{
    public static readonly IList<string> LegTypes = new ReadOnlyCollection<string>
    (
        new List<string> {
            "PICKUP",
            "PICKUP MT",
            "DELIVERY",
            "RETURN MT"
        }
    );

}