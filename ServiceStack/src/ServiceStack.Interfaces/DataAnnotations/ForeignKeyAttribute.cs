﻿using System;

namespace ServiceStack.DataAnnotations;

/// <summary>
/// Define an RDBMS Foreign Key Relationship
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ForeignKeyAttribute(Type type) : ReferencesAttribute(type)
{
    public string OnDelete { get; set; }
    public string OnUpdate { get; set; }
    /// <summary>
    /// Explicit foreign key name. If it's null, or empty, the FK name will be autogenerated as before.
    /// </summary>
    public string ForeignKeyName { get; set; }
}