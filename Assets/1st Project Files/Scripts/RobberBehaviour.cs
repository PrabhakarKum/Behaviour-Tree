using System;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{ 
    BehaviourTree tree;
    public GameObject door;
    public GameObject diamond;
    public GameObject van;
    
    NavMeshAgent agent;

    private enum ActionState { Idle, Working };
    private ActionState _actionState = ActionState.Idle;
    
    private Node.NodeStatus _treeStatus = Node.NodeStatus.Running;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        tree = new BehaviourTree(name);
        var steal = new Sequence("Steal Something");
        var goToDoor = new Leaf("Go To Door", GoToDoor);
        var goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        var goToVan  = new Leaf("Go To Van", GoToVan);
        
        steal.AddChild(goToDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);
        
        tree.PrintTree();
    }
    
    private Node.NodeStatus GoToDoor()
    {
        return GoToLocation(door.transform.position);
    }

    private Node.NodeStatus GoToDiamond()
    {
       return GoToLocation(diamond.transform.position);
    }
    
    private Node.NodeStatus GoToVan()
    {
       return GoToLocation(van.transform.position);
    }
    
    private Node.NodeStatus GoToLocation(Vector3 destination)
    {
       var distance = Vector3.Distance(transform.position, destination);
       
       if (_actionState == ActionState.Idle)
       {
           agent.SetDestination(destination);
           _actionState = ActionState.Working;
       }
       else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
       {
           _actionState = ActionState.Idle;
           return Node.NodeStatus.Failure;
       }
       else if (distance <= 2)
       {
           _actionState = ActionState.Idle;
           return Node.NodeStatus.Success;
       }
       
       return Node.NodeStatus.Running;
    }

    private void Update()
    {
        if(_treeStatus == Node.NodeStatus.Running)
            _treeStatus = tree.Process();
    }
}
