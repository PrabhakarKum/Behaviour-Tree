using System;
using System.Collections.Generic;
using UnityEngine;
   
public class BehaviourTree : Node
{
    public BehaviourTree(string nodeName) : base(nodeName)
    {
        NodeName = "Tree";
    }

    public override NodeStatus Process()
    {
        return Children[CurrentChild].Process();
    }

    struct NodeLevel
    {
        public int Level;
        public Node Node;
        
    }
    public void PrintTree()
    {
        string treePrintOut = "";
        Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
        Node currentNode = this;
        nodeStack.Push(new NodeLevel() { Level = 0, Node = currentNode });

        while (nodeStack.Count != 0)
        {
            NodeLevel nextNode = nodeStack.Pop();
            treePrintOut += new string ('-' , nextNode.Level ) + nextNode.Node.NodeName + "\n";
            
            for (int i = nextNode.Node.Children.Count - 1; i >= 0; i--)
            {
                nodeStack.Push(new NodeLevel { Level = nextNode.Level + 1, Node = nextNode.Node.Children[i]});
            }
        }
        
        Debug.Log(treePrintOut);
    }
}
