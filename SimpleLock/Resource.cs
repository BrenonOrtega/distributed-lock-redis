using System;
namespace SimpleLock
{
    public record Resource(string Name, string Type, DateTime UpdatedAt, string UpdatedBy);
}