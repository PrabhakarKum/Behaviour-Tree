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
        var goToDiamond = new Node("Go To Diamond");
        var goToVan  = new Node("Go To Van");
        
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);
        
        tree.PrintTree();
        
        agent.SetDestination(diamond.transform.position);
    }
}
