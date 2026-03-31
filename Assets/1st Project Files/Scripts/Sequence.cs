
public class Sequence : Node
{
    public Sequence(string nodeName)
    {
        NodeName = nodeName;
    }

    public override NodeStatus Process()
    {
        var childStatus = Children[CurrentChild].Process();
        
        switch (childStatus)
        {
            case NodeStatus.Running:
                return NodeStatus.Running;
            case NodeStatus.Failure:
                return childStatus;
        }

        CurrentChild++;

        if (CurrentChild >= Children.Count)
        {
            CurrentChild = 0;
            return NodeStatus.Success;
        }
        
        return NodeStatus.Running;
    }
}
