using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public enum NodeStatus { Success, Running, Failure };
    
    public readonly List<Node> Children = new List<Node>();

    protected int CurrentChild = 0;
    public string NodeName;

    protected Node(string nodeName = "Node")
    {
        NodeName = nodeName;
    }

    public virtual NodeStatus Process()
    {
       return Children[CurrentChild].Process();
    }
    
    public void AddChild(Node child)
    {
        Children.Add(child);
    }
}
