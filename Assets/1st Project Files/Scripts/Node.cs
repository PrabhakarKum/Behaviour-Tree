using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public enum Status { SUCCESS, RUNNING, FAILURE };
    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string nodeName;
    
    public Node(string nodeName)
    {
        this.nodeName = nodeName;
    }

    public void AddChild(Node child)
    {
        children.Add(child);
    }
}
