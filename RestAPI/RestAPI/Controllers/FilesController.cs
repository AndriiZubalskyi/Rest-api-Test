using RestAPI.Models;
using RestAPI.Services;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Helpers;
using MongoDB.Bson;

namespace RestAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly FileService _fileService;
    private readonly OperationService _operationService;

    public FilesController(FileService fileService, OperationService operationService)
    {
        _fileService = fileService;
        _operationService = operationService;
    }        

    [HttpGet]
    public async Task<List<FileModel>> Get()
    {        
        await _operationService.CreateAsync(new OperationModel { OperationFile = "ALL FILES", OperationName = nameof(Get) });
        return await _fileService.GetAsync();
    }        

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<FileModel>> Get(string id)
    {
        var file = await _fileService.GetAsync(id);

        if (file is null)
        {
            return NotFound();
        }
        await _operationService.CreateAsync(new OperationModel { OperationFile = file.FileName, OperationName = nameof(Get) });
        return file;
    }

    [HttpPost]
    public async Task<IActionResult> Post(FileModel newFile)
    {
        newFile.Text = Functions.TextEdit(newFile.Text);
        newFile.Id = ObjectId.GenerateNewId().ToString();
        await _fileService.CreateAsync(newFile);
        await _operationService.CreateAsync(new OperationModel { OperationFile = newFile.FileName, OperationName = nameof(Post) });
        return CreatedAtAction(nameof(Get), new { id = newFile.Id }, newFile);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, FileModel updatedFile)
    {
        var file = await _fileService.GetAsync(id);

        if (file is null)
        {
            return NotFound();
        }

        updatedFile.Id = file.Id;

        await _fileService.UpdateAsync(id, updatedFile);
        await _operationService.CreateAsync(new OperationModel { OperationFile = updatedFile.FileName, OperationName = nameof(Update) });

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var file = await _fileService.GetAsync(id);

        if (file is null)
        {
            return NotFound();
        }

        await _fileService.RemoveAsync(id);
        await _operationService.CreateAsync(new OperationModel { OperationFile = file.FileName, OperationName = nameof(Delete) });

        return NoContent();
    }
}
