using System;
using System.Collections;
using System.Collections.Generic;


namespace XmlModels;

public sealed class Tree : IEnumerable<KeyValuePair<string, Node>>
{
    public MxFile XmlModel { get; set; }

    private Dictionary<string, Node> _nodes = new();
    public Node Root { get; set; }

    internal void SetRoot()
    {
        Node root = null;
        foreach (var node in _nodes.Values)
        {
            if (node.Parent is null)
            {
                if (root is not null)
                    throw new Exception("Multi roots");
                root = node;
            }
        }

        Root = root;
    }

    internal void Add(string id, Node node)
    {
        _nodes[id] = node;
    }

    internal void ConnectFromTo(string from, string to, string value)
    {
        Node fromNode = _nodes[from];
        Node toNode = _nodes[to];

        fromNode.Children[value] = toNode;
        toNode.Parent = fromNode;
    }


    public IEnumerator<KeyValuePair<string, Node>> GetEnumerator()
    {
        return _nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}