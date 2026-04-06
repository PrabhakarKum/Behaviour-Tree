using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{ 
    protected BehaviourTree _tree;
    private NavMeshAgent _agent;

    public enum ActionState
    {
        Idle,
        Working
    };

    public ActionState _actionState = ActionState.Idle;
    public Node.NodeStatus _treeStatus = Node.NodeStatus.Running;
    private WaitForSeconds _waitForSeconds;

    protected virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _tree = new BehaviourTree(name);
        _waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 1f));
        
    }

    protected void StartBehaviour()
    {
        StartCoroutine(Behaviour());
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

    private IEnumerator Behaviour()
    {
        while (true)
        {
            if(_treeStatus != Node.NodeStatus.Success)
                _treeStatus = _tree.Process();
            
            yield return _waitForSeconds;
        }
    }
}
