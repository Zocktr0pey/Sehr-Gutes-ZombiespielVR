using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    [SerializeField] private bool takesDamage = false;
    [SerializeField] private bool pushable = false;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private UnityEvent audioManagerMethod;
    private float currentHealth;

    Rigidbody rb;

    void Awake()
    {
        Init(pushable, takesDamage, maxHealth);
        if (pushable)
        {
            rb = GetComponent<Rigidbody>();
        } 
    }

    public void Init(bool pushable, bool takesDamage, float maxHealth)
    {
        this.pushable = pushable;
        this.takesDamage = takesDamage;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void Hit(float amount, Vector3 direction, Vector3 hitPosition)
    {
        audioManagerMethod?.Invoke();
        if (takesDamage)
        {
            currentHealth -= amount;
        }
        if (pushable)
        {
            rb.AddForce(50 * direction * 1 / rb.mass);
        }
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public bool GetTakesDamage()
    {
        return takesDamage;
    }
}
