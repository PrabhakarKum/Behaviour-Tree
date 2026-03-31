
public class Leaf : Node
{
    public delegate NodeStatus Tick();
    public readonly Tick ProcessMethod;

    public Leaf(string nodeName, Tick processMethod) : base(nodeName)
    {
        NodeName = nodeName;
        ProcessMethod = processMethod;
    }
    public override NodeStatus Process()
    {
        return ProcessMethod?.Invoke() ?? NodeStatus.Failure;
    }
}