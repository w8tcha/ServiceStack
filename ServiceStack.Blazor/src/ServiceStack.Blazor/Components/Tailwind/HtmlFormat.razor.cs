﻿using Microsoft.AspNetCore.Components;

namespace ServiceStack.Blazor.Components.Tailwind;

/// <summary>
/// Formats any Serializable object in a human-friendly HTML Format
/// </summary>
public partial class HtmlFormat
{
    [Parameter, EditorRequired]
    public object? Value { get; set; }

    [Parameter]
    public string @class { get; set; } = CssDefaults.HtmlFormat.Class;
}
