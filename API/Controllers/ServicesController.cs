using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Common;
using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ServicesController : ControllerBase
{
    private readonly IServicesService _servicesService;

    public ServicesController(IServicesService servicesService)
    {
        _servicesService = servicesService;
    }

    // GET: api/services
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ServiceResponse>>>> GetAll()
    {
        var serviceResponses = await _servicesService.GetAllAsync();
        return ApiResponse<IEnumerable<ServiceResponse>>.Ok(serviceResponses);
    }

    // GET: api/services/id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<ServiceResponse>>> GetById(int id)
    {
        var serviceResponse = await _servicesService.GetByIdAsync(id);
        return ApiResponse<ServiceResponse>.Ok(serviceResponse);
    }

    // POST: api/services
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ServiceResponse>>> Create(ServiceCreateRequest serviceCreateRequest)
    {
        var createdService = await _servicesService.CreateAsync(serviceCreateRequest);
        return ApiResponse<ServiceResponse>.Created(createdService,nameof(GetById) + new { id = createdService.Id });
    }

    // PUT: api/services/id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<ServiceResponse>>> Update(int id, ServiceUpdateRequest serviceUpdateRequest)
    {
        var updatedService = await _servicesService.UpdateAsync(id,serviceUpdateRequest);
        return ApiResponse<ServiceResponse>.Ok(updatedService,"Updated service location uri: " + nameof(GetById) + new { id = updatedService.Id });
    }

    // DELETE: api/services/id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
    {
        await _servicesService.DeleteAsync(id);

        return ApiResponse<string>.NoContent($"Service with id:{id} deleted successfully");

    }
}
