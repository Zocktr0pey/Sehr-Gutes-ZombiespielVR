using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float pushForceFactor = 1f;
    [SerializeField] private float rotationInDegrees = 45;
    [SerializeField] private GameObject currentGun;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject vrRig;

    private CharacterController controller;
    private InputManager inputManager;
    private AudioManager audioManager;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Gun gun;

    // F�r rotation mit linken Stick
    private bool hasSnapped = false;
    private float snapThreshold = 0.8f;
    private float snapReset = 0.2f;

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

        Rotation();

        Movement();

        // Schiessen!
        // Einzelschuss
        if (gun != null)
        {
            Gun();
        }
    }

    void Rotation()
    {
        Vector2 input = inputManager.GetRightStick();
        
        if (!hasSnapped && Mathf.Abs(input.x) > snapThreshold)
        {
            Debug.Log("Get past this if " + input);
            if (input.x < 0)
            {
                vrRig.transform.eulerAngles -= new Vector3(0, rotationInDegrees, 0);
            }
            else
            {
                vrRig.transform.eulerAngles += new Vector3(0, rotationInDegrees, 0);
            }

            hasSnapped = true;
            return;
        }

        if (hasSnapped && input.magnitude < snapReset)
        {
            hasSnapped = false;
        }

    }

    void Movement()
    {
        moveInput = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        transform.eulerAngles = new Vector3(0, camera.transform.eulerAngles.y, 0);
        move = transform.TransformDirection(move);
        transform.eulerAngles = Vector3.zero;

        controller.Move(moveSpeed * Time.deltaTime * move);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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

        // Bestatter schaut dr�ber
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
