using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float currentHealth = 30f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float pushForceFactor = 1f;
    [SerializeField] private float rotationInDegrees = 45;
    [SerializeField] private GameObject currentGun;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject vrRig;
    [SerializeField] private GameObject magazine;
    [SerializeField] private FileManager fileManager;

    public Transform leftHand;
    public Transform rightHand;
    public float reloadDistance = 0.18f;
    public float magazineDistance = 0.6f;
    [Header("UI")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    

    private CharacterController controller;
    private InputManager inputManager;
    private AudioManager audioManager;
    private GameManager gameManager;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Gun gun;
    private bool canReload = true;
    private int currentWave = 0;


    // Punktzahl fur toten von Zombies
    private int score = 0;

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
        gameManager = GameManager.Instance;

        // Init stats
        currentHealth = maxHealth;

        // Init UI
        scoreText.text = $"Score: {this.score}";
        waveText.text = $"Wave: {this.currentWave}";
        lifeText.text = $"Life: {this.currentHealth}/{this.maxHealth}";

    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentWave != WaveManager.Instance.GetCurrentWave())
        {
            this.currentWave = WaveManager.Instance.GetCurrentWave();
            waveText.text = $"Wave: {this.currentWave}";
        }

        if (currentGun != null)
        {
            gun = currentGun.GetComponent<Gun>();
        }

        Rotation();
        Movement();

        float handDist = Vector3.Distance(leftHand.position, rightHand.position);

        if (handDist > magazineDistance)
        {
            canReload = true;
            magazine.SetActive(true);
        }

        // Schiessen!
        // Einzelschuss
        if (gun != null)
        {
            Gun(handDist);
        }
    }

    void Rotation()
    {
        Vector2 input = inputManager.GetRightStick();
        
        if (!hasSnapped && Mathf.Abs(input.x) > snapThreshold)
        {
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
        this.currentHealth -= damage;
        audioManager.PlayerDamage();

        lifeText.text = $"Life: {this.currentHealth}/{this.maxHealth}";

        // Bestatter schaut dr�ber
        if (currentHealth <= 0)
        {
            Debug.Log("Spieler ist Tod");
            Death();
        }
    }
    private void Death()
    {
        //Nochmal, vllt. klingts nett
        audioManager.PlayerDamage();
        fileManager.WriteScore(score);
        gameManager.GameOver();
    }

    void Gun(float handDist)
    {
        if (inputManager.GetSingleFire() && !gun.isFullAuto)
        {
            gun.Shoot();
            Debug.Log("Score: "+this.score);
        }

        // Dauerfeuer (wird bei JEDEM Frame getriggert)
        if (inputManager.GetContinuousFire() && gun.isFullAuto)
        {
            gun.Shoot();
        }

        if (handDist < reloadDistance && canReload && !gun.AmmoIsFull())
        {
            gun.Reload();
            canReload = false;
            magazine.SetActive(false);
        }
    }

    // Gibt aktuelle Punktzahl zuruck
    public int GetScore()
    {
        return this.score;
    }
    public void IncreaseScore()
    {
        this.score++;
        scoreText.text = $"Score: {this.score}";
    }
}
