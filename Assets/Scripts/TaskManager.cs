using NUnit.Framework;
using UnityEngine;

public enum TaskType
{
    None,
    Button,
    Pickup 
}

public class TaskManager : MonoBehaviour
{
    [Header("Tareas Disponibles")]
    public CharacterTaskHandler buttonTaskHandler;

    [Header("Tareas Actuales")]
    public bool IsBusy { get; private set; }
    private TaskType _currentTaskType = TaskType.None;

    public void SetBussy(bool busy)
    {
        IsBusy = busy;
    }

    public void AssignTask(TaskType taskType, Transform target)
    {
        if (IsBusy) return;

        switch (taskType)
        {
            case TaskType.Button:
                var button = target.GetComponent<ButtonInteractable>();
                if (buttonTaskHandler != null && button != null)
                {
                    buttonTaskHandler.MoveToButton(button);
                    _currentTaskType = TaskType.Button;
                }
                break;
            case TaskType.Pickup:
                
                break;
            default:
                Debug.LogWarning("Tipo de tarea no reconocido.");
                break;
        }
    }

    public void ClearCurrentTask()
    {
        _currentTaskType = TaskType.None;
        IsBusy = false;
    }
}
