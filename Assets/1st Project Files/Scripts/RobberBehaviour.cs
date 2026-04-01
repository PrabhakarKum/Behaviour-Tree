using System;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{ 
    private BehaviourTree tree;
    public GameObject frontDoor;
    public GameObject backDoor;
    public GameObject diamond;
    public GameObject van;
    
    private NavMeshAgent agent;

    private enum ActionState { Idle, Working };
    private ActionState _actionState = ActionState.Idle;
    
    private Node.NodeStatus _treeStatus = Node.NodeStatus.Running;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        tree = new BehaviourTree(name);
        
        var steal = new Sequence("Steal Something");
        
        var goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
        var goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        var goToBackDoor =  new Leaf("Go To Back Door", GoToBackDoor);
        var goToVan  = new Leaf("Go To Van", GoToVan);
        
        var openDoor = new Selector("Open Door");
        
        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);
        
        steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        
        tree.AddChild(steal);
        tree.PrintTree();
    }
    
    private Node.NodeStatus GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }
    
    private Node.NodeStatus GoToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    private Node.NodeStatus GoToDiamond()
    {
       var status =  GoToLocation(diamond.transform.position);
       
       if (status == Node.NodeStatus.Success)
       {
           diamond.transform.SetParent(gameObject.transform);
       }

       return status;
    }
    
    private Node.NodeStatus GoToVan()
    {
       return GoToLocation(van.transform.position);
    }

    public Node.NodeStatus GoToDoor(GameObject door)
    {
        var status = GoToLocation(door.transform.position);
        
        if (status == Node.NodeStatus.Success)
        {
            if (!door.GetComponent<Lock>().IsLocked)
            {
                door.SetActive(false);
                return Node.NodeStatus.Success;
            }
            return Node.NodeStatus.Failure;
        }

        return status;
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
