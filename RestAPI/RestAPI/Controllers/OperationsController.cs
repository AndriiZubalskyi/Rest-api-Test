using RestAPI.Models;
using RestAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperationsController : ControllerBase
{
    private readonly OperationService _operationService;

    public OperationsController(OperationService operationService) =>
        _operationService = operationService;

    [HttpGet]
    public async Task<List<OperationModel>> Get() =>
        await _operationService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<OperationModel>> Get(string id)
    {
        var operation = await _operationService.GetAsync(id);

        if (operation is null)
        {
            return NotFound();
        }

        return operation;
    }

    [HttpPost]
    public async Task<IActionResult> Post(OperationModel newOperation)
    {
        await _operationService.CreateAsync(newOperation);

        return CreatedAtAction(nameof(Get), new { id = newOperation.Id }, newOperation);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, OperationModel updatedOperation)
    {
        var operation = await _operationService.GetAsync(id);

        if (operation is null)
        {
            return NotFound();
        }

        updatedOperation.Id = operation.Id;

        await _operationService.UpdateAsync(id, updatedOperation);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var operation = await _operationService.GetAsync(id);

        if (operation is null)
        {
            return NotFound();
        }

        await _operationService.RemoveAsync(id);

        return NoContent();
    }
}
