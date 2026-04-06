using System.Collections.Generic;
public class Node
{
    public enum NodeStatus { Success, Running, Failure };
    
    public List<Node> Children = new List<Node>();

    protected int CurrentChild = 0;
    public string NodeName;
    public int SortOrder;

    protected Node(string nodeName = "Node")
    {
        NodeName = nodeName;
    }
    protected Node(string nodeName, int order)
    {
        NodeName = nodeName;
        SortOrder = order;
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
