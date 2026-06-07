using Microsoft.AspNetCore.Mvc;

public class StudentParamters
{
    [FromRoute]
    public int id { get; set; }

    [FromQuery]
    public string firstname { get; set; }

    [FromHeader]
    public string lastname { get; set; }
}
