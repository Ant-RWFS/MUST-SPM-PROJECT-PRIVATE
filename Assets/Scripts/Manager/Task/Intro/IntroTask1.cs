using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTask1 : Task
{
    public static IntroTask1 instance;

    public Inventory bag;
    public Item reward;
    private int taskItemIndexInBag;
    protected override void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;

        taskItemIndexInBag = -1;
        taskDescription = "Get 3 barrels of Gasoline from Dust Warriors. [0/3]";
    }

    protected override void Start()
    {
        base.Start();
        TaskManager.instance.tasks.Add(this);
    }

    protected override void Update()
    {
        base.Update();
        DetectGasolineInBag();
        DestroyLogic();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (taskItemIndexInBag != -1)
        {
            if (bag.itemList[taskItemIndexInBag].heldAmount >= 3)
                bag.itemList[taskItemIndexInBag].heldAmount -= 3;

            if (bag.itemList[taskItemIndexInBag].heldAmount <= 0)
                bag.itemList.Remove(bag.itemList[taskItemIndexInBag]);
        }
       
        bag.AddItem(reward);
        TaskManager.instance.tasks.Remove(this);
    }
    private void DetectGasolineInBag()
    {
        if (taskItemIndexInBag < 0)
        {
            for (int i = 0; i < bag.itemList.Count; i++)
            {
                if (bag.itemList[i].name == "Gasoline")
                {
                    taskItemIndexInBag = i;
                    break;
                }
            }
        }
        else if (bag.itemList[taskItemIndexInBag].heldAmount < 3) 
        {
            taskDescription = $"Get 3 barrels of Gasoline from Dust Warriors. [{bag.itemList[taskItemIndexInBag].heldAmount}/3]";
        }
        else if (bag.itemList[taskItemIndexInBag].heldAmount >= 3)
        {
            taskDescription = "Get 3 barrels of Gasoline from Dust Warriors. [Done]";
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
