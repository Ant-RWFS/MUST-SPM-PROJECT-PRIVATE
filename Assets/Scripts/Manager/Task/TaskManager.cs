using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    const int maxDisplayedTasks = 4;
    public GameObject mutexUI;
    public static TaskManager instance;

    public GameObject taskUI;
    public GameObject taskContent;
    public TextMeshProUGUI taskText;
    
    public List<Task> tasks;


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;

        taskText = taskContent.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        AdjustTaskText();
    }

    private void Update()
    {
        AdjustTaskText();
        HideUI();
    }
    private void AdjustTaskText() 
    {
        if (tasks.Count <= maxDisplayedTasks)
        {
            string combinedTasks = string.Join("\n", tasks.Select(task => task.taskDescription));
            taskText.text = combinedTasks;
        }
        else
        {
            string combinedTasks = string.Join("\n", tasks.GetRange(0, maxDisplayedTasks).Select(task => task.taskDescription));
            combinedTasks += "\n...";

            taskText.text = combinedTasks;
        }
    }

    private void HideUI() 
    {
        if (mutexUI.activeSelf == true)
            taskUI.SetActive(false);
        else if (Input.GetKeyDown(KeyCode.Tab))
            taskUI.SetActive(!taskUI.activeSelf);
    }
}
