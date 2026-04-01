using System;
using UnityEngine;
using UnityEngine.AI;
using VHierarchy.Libs;

public class RobberBehaviour : MonoBehaviour
{ 
    private BehaviourTree _tree;
    public GameObject frontDoor;
    public GameObject backDoor;
    public GameObject diamond;
    public GameObject van;
    
    [Range(0,1000)]
    public int money = 800;
    
    private NavMeshAgent _agent;

    private enum ActionState { Idle, Working };
    private ActionState _actionState = ActionState.Idle;
    
    private Node.NodeStatus _treeStatus = Node.NodeStatus.Running;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        
        _tree = new BehaviourTree(name);
        
        var steal = new Sequence("Steal Something");
        
        var hasGotMoney = new Leaf("Has Got Money", HasMoney);
        
        var openDoor = new Selector("Open Door");
        var goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
        var goToBackDoor =  new Leaf("Go To Back Door", GoToBackDoor);
        
        var goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        
        var goToVan  = new Leaf("Go To Van", GoToVan);
        
        steal.AddChild(hasGotMoney);
        
        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);
        
        steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        
        _tree.AddChild(steal);
        _tree.PrintTree();
    }
    
    private Node.NodeStatus HasMoney()
    {
        return money > 500 ? Node.NodeStatus.Failure : Node.NodeStatus.Success;
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
       var status =  GoToLocation(van.transform.position);
       
       if (status == Node.NodeStatus.Success)
       {
           diamond.Destroy(); 
           money += 300;
       }

       return status;
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
           _agent.SetDestination(destination);
           _actionState = ActionState.Working;
       }
       else if (Vector3.Distance(_agent.pathEndPosition, destination) >= 2)
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
        if(_treeStatus != Node.NodeStatus.Success)
            _treeStatus = _tree.Process();
    }
}
