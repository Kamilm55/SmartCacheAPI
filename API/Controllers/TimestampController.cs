using Microsoft.AspNetCore.Mvc;
using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common;
using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;

namespace SmartCacheManagementSystem.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class TimestampController : ControllerBase
{
    private readonly ITimestampService _timestampService;

    public TimestampController(ITimestampService timestampService)
    {
        _timestampService = timestampService;
    }

    [HttpPost("verify")]
    public async Task<ActionResult<ApiResponse<TimestampResponse>>> VerifyChanges([FromBody] TimestampRequest request)
    {
        var currentChecksum = await _timestampService.VerifyChanges(request);
        return ApiResponse<TimestampResponse>.Ok(currentChecksum);
    }

}