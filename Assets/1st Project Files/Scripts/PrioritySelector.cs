using System.Collections.Generic;

public class PrioritySelector : Node
{
    private Node[] _nodeArray;
    public PrioritySelector(string nodeName)
    {
        NodeName = nodeName;
    }
    public override NodeStatus Process()
    {
        OrderNodes();
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

    private void OrderNodes()
    {
        _nodeArray = Children.ToArray();
        Sort(_nodeArray, 0, _nodeArray.Length - 1);
        Children = new List<Node>(_nodeArray);
    }
    
    private void Sort(Node[] array, int low, int high)
    {
        if (low < high)
        {
            var partitionIndex = Partition(array, low, high);
            Sort(array, low, partitionIndex - 1);
            Sort(array, partitionIndex + 1, high);
        }
    }

    private int Partition(Node[] array, int low, int high)
    {
        var pivot = array[high];
        var lowIndex = (low - 1);

        //2. Reorder the collection.
        for (var i = low; i < high; i++)
        {
            if (array[i].SortOrder <= pivot.SortOrder)
            {
                lowIndex++;

                (array[lowIndex], array[i]) = (array[i], array[lowIndex]);
            }
        }

        (array[lowIndex + 1], array[high]) = (array[high], array[lowIndex + 1]);

        return lowIndex + 1;
    }
    
}