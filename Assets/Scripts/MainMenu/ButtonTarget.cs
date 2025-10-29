using UnityEngine;
using UnityEngine.Events;

public class ButtonTarget : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private bool pushable = true;
    [SerializeField] private bool pushed = false;

    [Header("Event to Trigger When Pressed")]
    [SerializeField] private UnityEvent onPressed;

    //[Header("Sounds")]

    void Awake()
    {
        Init(pushable, pushed);
    }

    public void Init(bool pushable, bool pushed)
    {
        this.pushable = pushable;
        this.pushed = pushed;
    }

    public void PushButton()
    {
        if (!pushable || pushed) 
        {
            return;
        }

        // Sound (Click sonud)

        // Invoke Aktionen
        onPressed?.Invoke();
    }

    public bool GetState()
    {
        return pushed;
    }
}
