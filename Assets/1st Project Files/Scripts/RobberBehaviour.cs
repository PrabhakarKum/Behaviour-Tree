using System;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{ 
    BehaviourTree tree;
    public GameObject diamond;
    public GameObject van;
    
    NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        tree = new BehaviourTree(name);
        var steal = new Node("Steal Something");
        var goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        var goToVan  = new Leaf("Go To Van", GoToVan);
        
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);
        
        tree.PrintTree();
        tree.Process();

    }

    private Node.NodeStatus GoToDiamond()
    {
        agent.SetDestination(diamond.transform.position);
        return Node.NodeStatus.Success;
    }
    
    private Node.NodeStatus GoToVan()
    {
        agent.SetDestination(van.transform.position);
        return Node.NodeStatus.Success;
    }
}
