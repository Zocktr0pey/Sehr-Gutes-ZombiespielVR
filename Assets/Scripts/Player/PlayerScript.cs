using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float pushForceFactor = 1f;

    private CharacterController controller;
    private InputManager inputManager;
    private Vector2 moveInput;
    private Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
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

        // Dauerfeuer
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
}
