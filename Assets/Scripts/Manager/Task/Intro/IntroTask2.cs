using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTask2 : Task
{
    public static IntroTask2 instance;

    public Inventory bag;
    private int taskItemIndexInBag;

    protected override void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;

        taskItemIndexInBag = -1;
        taskDescription = "Get 1 Dynamo from Dust Jumper. [0/1]";
    }
    protected override void Start()
    {
        base.Start();
        TaskManager.instance.tasks.Add(this);
    }
    protected override void Update()
    {
        base.Update();
        DetectDynamoInBag();
        DestroyLogic();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (bag.itemList[taskItemIndexInBag].heldAmount>=1)
            bag.itemList[taskItemIndexInBag].heldAmount -= 1;

        if (bag.itemList[taskItemIndexInBag].heldAmount <= 0)
            bag.itemList.Remove(bag.itemList[taskItemIndexInBag]);

        TaskManager.instance.tasks.Remove(this);
    }
    private void DetectDynamoInBag()
    {
        if (taskItemIndexInBag < 0)
        {
            for (int i = 0; i < bag.itemList.Count; i++)
            {
                if (bag.itemList[i].name == "Dynamo")
                {
                    taskItemIndexInBag = i;
                    break;
                }
            }
        }
        else if (bag.itemList[taskItemIndexInBag].heldAmount < 1)
        {
            taskDescription = $"Get 1 Dynamo from Dust Jumper. [{bag.itemList[taskItemIndexInBag].heldAmount}/1]";
        }
        else if (bag.itemList[taskItemIndexInBag].heldAmount >= 1)
        {
            taskDescription = "Get 1 Dynamo from Dust Jumper. [Done]";
            client.isTaskAssigned = false;
        }
    }
    private void DestroyLogic()
    {
        if (client.isTaskAssigned == false)
        {
            if (client.plotInfo[client.currentPlotIndex].type != 2)
                Destroy(this.gameObject);
        }
    }
}
