using Microsoft.AspNetCore.Mvc;
using WebApplicationSample.Abstraction;
using WebApplicationSample.Persistence;
using WebApplicationSample.Persistence.Entities;

namespace WebApplicationSample.Controllers;

[ApiController]
[Route("[controller]")]
public class SchoolController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public SchoolController(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> CreateStudent([FromQuery] StudentQuery query)
    {
        var res = await dbContext.Students
            .ApplyFilter(query)
            .ApplySort(query)
            .ToPagedListAsync(query);

        return Ok(res);
    }

    public abstract class FilterableQuery : IPagination, IFilterable, ISortable
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? FilterBy { get; set; }
        public string? SortBy { get; set; }
    }

    public class StudentQuery : FilterableQuery
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}