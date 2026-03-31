
public class Leaf : Node
{
    public delegate NodeStatus Tick();
    private readonly Tick _processMethod;

    public Leaf(string nodeName, Tick processMethod) : base(nodeName)
    {
        NodeName = nodeName;
        _processMethod = processMethod;
    }
    public override NodeStatus Process()
    {
        return _processMethod?.Invoke() ?? NodeStatus.Failure;
    }
}