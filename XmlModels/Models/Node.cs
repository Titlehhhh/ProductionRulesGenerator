using System.Collections.Generic;

namespace XmlModels;

public abstract class Node
{
    public MxCell OriginalCell { get; set; }
    
    public int Id { get; set; }
    public Node? Parent { get; set; }
    public Dictionary<string, Node> Children { get; set; } = new();
}