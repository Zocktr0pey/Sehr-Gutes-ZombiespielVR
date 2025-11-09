using System.Reflection;
using System.Collections; // neu
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
    AudioManager audioManager;
    private bool isDying = false;
    

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

        // assign audio manager (singleton or find)
        audioManager = AudioManager.Instance ?? FindObjectOfType<AudioManager>();
        if (audioManager == null) Debug.LogWarning("AudioManager not found. Add AudioManager to the scene or assign it.");

        // assign animator
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogWarning("Animator not found on Zombie. Add Animator component or remove animator usages.");

        maxHealth = targetSelf.GetMaxHealth();

        onCooldown = false;
        targetSelf.audioManagerMethod.AddListener(AudioManager.Instance.ZombieDamage);

        //animator = GetComponent<Animator>();
        //animator.applyRootMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDying) return;
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
        playerScript.IncreaseScore();
        // audioManager.ZombieDeath();
        // animator.SetTrigger("DeathTrigger");
        // Geld oder Punkt f�r den Spieler
        // Zombiecount--
        Destroy(this.gameObject);
        if (isDying) return;
        isDying = true;

        // Sound
        audioManager.ZombieDeath();

        // Trigger death animation
        animator.SetTrigger("DeathTrigger");

        // Deaktiviere Kollisions- / Bewegungs-Effekte
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        if (rb != null) rb.isKinematic = true;

        // Warte auf das Ende der Death-Animation und zerstöre dann das Objekt
        StartCoroutine(PlayDeathAndDestroy());
    }

    private IEnumerator PlayDeathAndDestroy()
    {
        float waitTime = 1f;

        // if (animator != null)
        // {
        //     // Suche eine Animation mit "death" im Namen oder benutze die aktuelle State-Länge
        //     var ac = animator.runtimeAnimatorController;
        //     if (ac != null)
        //     {
        //         foreach (var clip in ac.animationClips)
        //         {
        //             if (clip.name.ToLower().Contains("death"))
        //             {
        //                 waitTime = clip.length;
        //                 break;
        //             }
        //         }
        //     }

        //     // fallback: versuche die aktuelle State-Länge (kann 0 sein)
        //     var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //     if (stateInfo.length > 0) waitTime = Mathf.Max(waitTime, stateInfo.length);
        // }

        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
