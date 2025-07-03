namespace SmartCacheManagementSystem.Common.Exceptions;


public class NotFoundException : Exception
{
    public NotFoundException(string key,int id)
        : base($"{key} not found with id:{id}")
    {
        
    }
    
    public NotFoundException(string msg)
        : base(msg)
    {
        
    }
}