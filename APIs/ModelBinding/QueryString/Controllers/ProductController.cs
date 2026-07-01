using Microsoft.AspNetCore.Mvc;

namespace QueryString.Controllers;

[ApiController]
public class ProductController : ControllerBase
{
    [HttpGet("Products-Controller")]
    public IActionResult Get(int id)
    {
        return Ok($"the product with id {id} is exist.");
    }

    [HttpGet("Products-Controller-1")]
    public IActionResult GetFromQuery([FromQuery(Name = "id")] int identifier)
    {
        return Ok($"the product with id {identifier} is exist.");
    }

    [HttpGet("bools-Controller-array")]
    //in api Controller based use the FromQuery when ever the recieved is compex object
    public IActionResult GetBoolsArray([FromQuery] bool[] bools)
    {
        return Ok(bools);
    }

    // no need to use the AsParameters in Controller Based api
    // but why?
    [HttpGet("daterange-Controller-complex")]
    public IActionResult GetDateRange([FromQuery] DateRangeQuery daterange)
    {
        return Ok(daterange);
    }

    [HttpGet("daterangequery-Controller-complexquery")]
    public IActionResult GetDateRangeComplex(DateRangeComplexQuery daterange)
    {
        return Ok(daterange);
    }
}
