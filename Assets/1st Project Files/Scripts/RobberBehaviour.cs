using System;
using UnityEngine;

public class RobberBehaviour : MonoBehaviour
{ 
    BehaviourTree tree;
    private void Start()
    {
        tree = new BehaviourTree(name);
        var steal = new Node("Steal Something");
        var goToDiamond = new Node("Go To Diamond");
        var goToVan  = new Node("Go To Van");
        
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);
        
        tree.PrintTree();
        
    }
}
