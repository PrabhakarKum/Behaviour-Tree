using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BTAgent
{
    public GameObject frontDoor;
    public GameObject backDoor;
    public GameObject diamond;
    public GameObject painting;
    public GameObject van;

    [Range(0, 1000)] public int money = 800;

    private GameObject _pickup;

    protected override void Start()
    {
        base.Start();

        var steal = new Sequence("Steal Something");

        var hasGotMoney = new Leaf("Has Got Money", HasMoney);

        var openDoor = new PrioritySelector("Open Door");

        var invertMoney = new Inverter("Invert Money");

        var goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor, 2);
        var goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor, 1);

        var chooseItem = new PrioritySelector("Choose Item");
        var goToDiamond = new Leaf("Go To Diamond", GoToDiamond, 1);
        var goToPainting = new Leaf("Go To Painting", GoToPainting, 2);

        var goToVan = new Leaf("Go To Van", GoToVan);

        invertMoney.AddChild(hasGotMoney);
        steal.AddChild(invertMoney);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);
        steal.AddChild(openDoor);
        
        chooseItem.AddChild(goToPainting);
        chooseItem.AddChild(goToDiamond);
        steal.AddChild(chooseItem);
        
        steal.AddChild(goToVan);

        _tree.AddChild(steal);
        _tree.PrintTree();
        StartBehaviour();
    }

    private Node.NodeStatus HasMoney()
    {
        return money < 500 ? Node.NodeStatus.Failure : Node.NodeStatus.Success;
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
        if(!diamond.activeSelf) return Node.NodeStatus.Failure;
        
        var status = GoToLocation(diamond.transform.position);

        if (status == Node.NodeStatus.Success)
        {
            diamond.transform.SetParent(gameObject.transform);
            _pickup = diamond;
        }

        return status;
    }
    
    private Node.NodeStatus GoToPainting()
    {
        if(!painting.activeSelf) return Node.NodeStatus.Failure;
        
        var status = GoToLocation(painting.transform.position);

        if (status == Node.NodeStatus.Success)
        {
            painting.transform.SetParent(gameObject.transform);
            _pickup = painting;
        }

        return status;
    }

    private Node.NodeStatus GoToVan()
    {
        var status = GoToLocation(van.transform.position);

        if (status == Node.NodeStatus.Success)
        {
            _pickup.transform.SetParent(van.transform);
            _pickup.SetActive(false);
            money += 300;
        }

        return status;
    }

    private Node.NodeStatus GoToDoor(GameObject door)
    {
        var status = GoToLocation(door.transform.position);

        if (status == Node.NodeStatus.Success)
        {
            if (!door.GetComponent<Lock>().IsLocked)
            {
                door.GetComponent<NavMeshObstacle>().enabled = false;
                return Node.NodeStatus.Success;
            }

            return Node.NodeStatus.Failure;
        }

        return status;
    }
}