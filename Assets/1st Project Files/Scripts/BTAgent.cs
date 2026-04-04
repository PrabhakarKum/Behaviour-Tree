using System;
using UnityEngine;
using UnityEngine.AI;
using VHierarchy.Libs;

public class BTAgent : MonoBehaviour
{ 
    protected BehaviourTree _tree;
    protected NavMeshAgent _agent;

    protected enum ActionState
    {
        Idle,
        Working
    };

    protected ActionState _actionState = ActionState.Idle;
    protected Node.NodeStatus _treeStatus = Node.NodeStatus.Running;

    protected virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _tree = new BehaviourTree(name);
    }

    protected Node.NodeStatus GoToLocation(Vector3 destination)
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

    protected virtual void Update()
    {
        if (_treeStatus != Node.NodeStatus.Success) _treeStatus = _tree.Process();
    }
}
