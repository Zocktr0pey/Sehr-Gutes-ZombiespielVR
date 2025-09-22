using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float pushForceFactor = 1f;
    [SerializeField] private GameObject currentGun;

    private CharacterController controller;
    private InputManager inputManager;
    private AudioManager audioManager;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Gun gun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        audioManager = AudioManager.Instance;

        // Init stats
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        gun = currentGun.GetComponent<Gun>();

        moveInput = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        move = transform.TransformDirection(move);

        controller.Move(moveSpeed * Time.deltaTime * move);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Schiessen!
        // Einzelschuss
        if (gun != null)
        {
            Gun();
        }
    }

    // andere Rigidbodys wegkicken
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb != null && !rb.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            float pushForce = pushForceFactor * 1/rb.mass;

            rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }

    // Spieler bekommt Schaden (durch Zombie in der Regel)
    public void TakeDamage(float damage) 
    { 
        currentHealth -= damage;
        audioManager.PlayerDamage();

        // Bestatter schaut drüber
        if (currentHealth <= 0)
        {
            //audioManager.PlayerDeath()
            //gameManager.GameOver();
        }
    }

    void Gun()
    {
        if (inputManager.GetSingleFire() && !gun.isFullAuto)
        {
            gun.Shoot();
        }

        // Dauerfeuer (wird bei JEDEM Frame getriggert)
        if (inputManager.GetContinuousFire() && gun.isFullAuto)
        {
            gun.Shoot();
        }

        if (inputManager.GetReload())
        {
            gun.Reload();
        }
    }
}
