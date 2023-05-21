using Microsoft.AspNetCore.Mvc;
using WebApplicationSample.Abstraction;
using WebApplicationSample.Persistence.Entities;
using WebApplicationSample.Persistence.Repositories;

namespace WebApplicationSample.Controllers;

[ApiController]
[Route("[controller]")]
public class SchoolController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudentRepository _studentRepository;

    public SchoolController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _studentRepository = _unitOfWork.GetRepository<IStudentRepository>();
    }

    [HttpPost("CreateStudent")]
    public async Task<IActionResult> CreateStudent([FromBody] Student student)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _studentRepository.InsertAsync(student);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
            return Ok();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    [HttpPost("CreateStudent2")]
    public async Task<IActionResult> CreateStudent2([FromBody] Student student, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.CommitTransactionAsync(async (token) =>
            {
                await _studentRepository.InsertAsync(student, token);
            }, cancellationToken);

            return Ok();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}