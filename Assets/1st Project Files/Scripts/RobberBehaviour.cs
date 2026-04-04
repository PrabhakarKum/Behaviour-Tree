using System;
using UnityEngine;
using UnityEngine.AI;
using VHierarchy.Libs;

public class RobberBehaviour : BTAgent
{
    public GameObject frontDoor;
    public GameObject backDoor;
    public GameObject diamond;
    public GameObject van;

    [Range(0, 1000)] public int money = 800;

    protected override void Start()
    {
        base.Start();

        var steal = new Sequence("Steal Something");

        var hasGotMoney = new Leaf("Has Got Money", HasMoney);

        var openDoor = new Selector("Open Door");

        var invertMoney = new Inverter("Invert Money");

        var goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
        var goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor);

        var goToDiamond = new Leaf("Go To Diamond", GoToDiamond);

        var goToVan = new Leaf("Go To Van", GoToVan);

        invertMoney.AddChild(hasGotMoney);
        steal.AddChild(invertMoney);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);
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
        var status = GoToLocation(diamond.transform.position);

        if (status == Node.NodeStatus.Success)
        {
            diamond.transform.SetParent(gameObject.transform);
        }

        return status;
    }

    private Node.NodeStatus GoToVan()
    {
        var status = GoToLocation(van.transform.position);

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
}