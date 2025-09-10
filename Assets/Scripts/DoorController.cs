using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator animator;
    private bool _opened = false;

    public bool IsOpened => _opened;

    public void OpenDoor()
    {
        if (!_opened)
        {
            animator.SetBool("isOpen", true); 
            _opened = true;
            Debug.Log("Si, llame a DoorController");
        }
    }
}
