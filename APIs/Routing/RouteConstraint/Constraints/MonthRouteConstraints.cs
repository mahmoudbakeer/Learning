namespace RouteConstraint.Constraints;

public class MonthRouteConstraint : IRouteConstraint
{
    public bool Match(
        HttpContext? httpContext,
        IRouter? route,
        string routeKey,
        RouteValueDictionary values,
        RouteDirection routeDirection
    )
    {
        if (!values.TryGetValue(routeKey, out var MonthNumber))
        {
            return false;
        }
        else if (int.TryParse(MonthNumber.ToString(), out int Month))
            return Month >= 1 && Month <= 12;
        return true;
    }
}
