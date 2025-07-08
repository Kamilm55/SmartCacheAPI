/*using Microsoft.AspNetCore.Mvc;
using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common;
using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;

namespace SmartCacheManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ChecksumController : ApiControllerBase
{
    private readonly IChecksumService _checksumService;

    public ChecksumController(IChecksumService checksumService)
    {
        _checksumService = checksumService;
    }

    [HttpPost("checksum/categories/verify")]
    public async Task<ActionResult<ApiResponse<ChecksumResponse>>> VerifyCategoriesChecksum([FromBody] string checksum)
    {
        var currentChecksum = await _checksumService.GetChecksumAsync(checksum);
        return ApiResponse<ChecksumResponse>.Ok(currentChecksum);
    }

}*/