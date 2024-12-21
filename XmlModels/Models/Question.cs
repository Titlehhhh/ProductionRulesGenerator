using System.Diagnostics;

namespace XmlModels;

[DebuggerDisplay("{Text}")]
public sealed class Question : Node
{
    public string Text { get; set; }
    public string VarName { get; set; }
}