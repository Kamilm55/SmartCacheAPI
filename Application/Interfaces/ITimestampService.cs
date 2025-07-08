using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;

namespace SmartCacheManagementSystem.Application.Interfaces;

public interface ITimestampService
{
    Task<TimestampResponse> VerifyChanges(TimestampRequest request);
}