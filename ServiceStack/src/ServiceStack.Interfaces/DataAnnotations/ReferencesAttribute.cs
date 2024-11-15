using System;

namespace ServiceStack.DataAnnotations;

/// <summary>
/// Document a reference to an external Type, used to create simple Foreign Key references
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class ReferencesAttribute(Type type) : AttributeBase
{
    public Type Type { get; set; } = type;
}