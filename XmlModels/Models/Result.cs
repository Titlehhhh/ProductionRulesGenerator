using System.Diagnostics;

namespace XmlModels;

[DebuggerDisplay("{Name}")]
public sealed class Result : Node
{
    public string Name { get; set; }
}