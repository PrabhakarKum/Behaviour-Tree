public class RandomSelector : Node
{
    public RandomSelector(string nodeName)
    {
        NodeName = nodeName;
    }

    public override NodeStatus Process()
    {
        Children.Shuffle(); 
        var childStatus = Children[CurrentChild].Process();
        
        switch (childStatus)
        {
            case NodeStatus.Running:
                return NodeStatus.Running;
            case NodeStatus.Success:
                CurrentChild = 0;
                return NodeStatus.Success;
        }

        CurrentChild++;

        if (CurrentChild >= Children.Count)
        {
            CurrentChild = 0;
            return NodeStatus.Failure;
        }
        
        return NodeStatus.Running;
    }
}