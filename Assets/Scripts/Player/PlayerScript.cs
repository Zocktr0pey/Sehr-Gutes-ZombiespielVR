using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float pushForceFactor = 1f;
    [SerializeField] private GameObject currentGun;

    [Header("VR References")]
    [SerializeField] private XROrigin xrOrigin;
    // Hauptkamera in XR Origin
    [SerializeField] private Transform headTransform;
    // Für die Kollisionen
    [SerializeField] private LayerMask collisionMask;

    private CharacterController controller;
    private InputManager inputManager;
    private AudioManager audioManager;
    private Gun gun;
    private Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        audioManager = AudioManager.Instance;

        // Init stats
        currentHealth = maxHealth;

        if (xrOrigin == null)
            xrOrigin = FindAnyObjectByType<XROrigin>();

        if (headTransform == null && xrOrigin != null)
            headTransform = xrOrigin.Camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        gun = currentGun.GetComponent<Gun>();

        HandleMovement();
        ApplyGravity();
        KeepHeadInsideCollider();

        // Schiessen!
        // Einzelschuss
        if (gun != null)
        {
            GunInput();
        }
    }
    private void HandleMovement()
    {
        // Controller Bewegungs-Eingabe
        Vector2 moveInput = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        if (headTransform != null)
        {
            Vector3 forward = headTransform.forward;
            Vector3 right = headTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            // Finale Bewegung
            move = (forward * move.y + right * move.x);
        }

        controller.Move(moveSpeed * Time.deltaTime * move);
    }
    private void ApplyGravity()
    {
        // Bei Bodenkontakt, hat der Spieler geringe Gravität statt 0 (verhindert bouncen/schweben)
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -1f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void KeepHeadInsideCollider()
    {
        if (headTransform == null)
            return;

        Vector3 headLocal = transform.InverseTransformPoint(headTransform.position);
        Vector3 horizontalOffset = new Vector3(headLocal.x, 0, headLocal.z);

        // Bewegt den Körper zur Headset-Bewegung, wenn aus dem Collider
        if (horizontalOffset.magnitude > controller.radius)
        {
            Vector3 correction = transform.TransformVector(horizontalOffset.normalized * (horizontalOffset.magnitude - controller.radius));
            transform.position += correction;
        }

        // Drückt Spieler aus Colliders, wenn der Lümmel reinpeakt
        if (Physics.CheckSphere(headTransform.position, 0.15f, collisionMask))
        {
            Vector3 pushBack = (transform.position - headTransform.position).normalized * 0.05f;
            transform.position += pushBack;
        }
    }

    // andere Rigidbodys wegkicken
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb != null && !rb.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            float pushForce = pushForceFactor * (1/rb.mass);

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

    void GunInput()
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
