using Unity.Netcode;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : NetworkBehaviour
{
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float pushForceFactor = 1f;
    [SerializeField] private Camera cam;

    private CharacterController controller;
    private InputManager inputManager;
    private AudioManager audioManager;
    private NetworkShit networkShit;
    private Vector2 moveInput;
    private Vector3 velocity;

    public override void OnNetworkSpawn()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        audioManager = AudioManager.Instance;
        networkShit = NetworkShit.Instance;
        networkShit.AddPlayer(this.gameObject);

        // Init stats
        currentHealth = maxHealth;

        if (!IsOwner)
        {
            cam.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        moveInput = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        move = transform.TransformDirection(move);

        controller.Move(moveSpeed * Time.deltaTime * move);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Schiessen!
        // Einzelschuss
        if (inputManager.GetSingleFire())
        {
            Debug.Log("Pew Pew");
            // Hier Einzeschusswaffen.shoot()
        }

        // Dauerfeuer (wird bei JEDEM Frame getriggert)
        if (inputManager.GetContinuousFire())
        {
            // Hier Dauerfeuerwaffen.shoot()
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
}
