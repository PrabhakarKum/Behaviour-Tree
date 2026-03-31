using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public enum NodeStatus { Success, Running, Failure };
    
    public NodeStatus nodeStatus;
    public List<Node> Children = new List<Node>();
    
    public int CurrentChild = 0;
    public string NodeName;
    
    public Node(string nodeName = "Node")
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
