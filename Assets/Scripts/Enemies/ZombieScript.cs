using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    private float currentHealth;
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float rotationSpeed = 3f;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float attackRange = 1.5f;
    [SerializeField]
    private float attackCooldown = 2f;
    private float timeSinceLastAttack;
    private bool onCooldown;
    private Animator animator;

    private GameObject player;
    private PlayerScript playerScript;
    private Rigidbody rb;
    private Target targetSelf;
    Vector3 positionDiff;
    Vector3 playerDirection;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetSelf = GetComponent<Target>();
        if (targetSelf == null)
        {
            Debug.LogError("Please Add Target script as a component and set the \"Takes Damage\" flag to true. Add the Audiomanager Gameobject and its ZombieDamage() Method. Thanks");
        }
        targetSelf.Init(false, true, maxHealth);

        
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
        rb = GetComponent<Rigidbody>();

        maxHealth = targetSelf.GetMaxHealth();

        onCooldown = false;
        targetSelf.audioManagerMethod.AddListener(AudioManager.Instance.ZombieDamage);

        //animator = GetComponent<Animator>();
        //animator.applyRootMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Health update and isAlive check
        currentHealth = targetSelf.GetHealth();
        if (currentHealth <= 0 )
        {
            Death();
        }

        ReduceCooldown();

        positionDiff = (player.transform.position - transform.position);

        if (positionDiff.magnitude <= attackRange && !onCooldown)
        {
            Attack();
        }

        Rotate();

        if (positionDiff.magnitude > attackRange) // Zombie bewegt sich nur wenn der Spieler au�erhalb der damageRange ist
        {
            Move();
        }
    }

    private void Attack()
    {
        playerScript.TakeDamage(damage);
        onCooldown = true;
    }

    private void Move()
    {
        // Forw�rtsbewegung
        Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    private void ReduceCooldown()
    {
        if (onCooldown)
        {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack > attackCooldown)
            {
                onCooldown = false;
                timeSinceLastAttack = 0;
            }
        }
    }

    private void Rotate()
    {
        // Richtungsvektor zum Spieler au�er y damit er sich nur um die Y - Achse dreht
        playerDirection = positionDiff;
        playerDirection.y = 0;
        playerDirection = playerDirection.normalized;

        // Zielrotation
        Quaternion lookRotation = Quaternion.LookRotation(playerDirection);

        // Smoothe Drehung
        transform.rotation = Quaternion.Slerp(
            transform.rotation,                     // Aktuelle rotation
            lookRotation,                           // Zielrotation
            Time.deltaTime * rotationSpeed          // Drehgeschwindigkeit
        );
    }

    private void Death()
    {
        // audioManager.ZombieDeath();
        // animator.SetTrigger("DeathTrigger");
        // Geld oder Punkt f�r den Spieler
        // Zombiecount--
        Destroy(this.gameObject);
    }
}
