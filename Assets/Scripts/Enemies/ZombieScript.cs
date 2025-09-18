using Unity.VisualScripting;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
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
    private bool onCoolDown;

    private GameObject player;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        onCoolDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (onCoolDown)
        {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack > attackCooldown)
            {
                onCoolDown = false;
                timeSinceLastAttack = 0;
            }
        }

        Vector3 positionDiff = (player.transform.position - transform.position);

        if (positionDiff.magnitude <= attackRange && !onCoolDown)
        {

            Debug.Log("You just got touched by a Zombie son~");
            // damage player
            // ...
            onCoolDown = true;
        }

        // Richtungsvektor zum Spieler auﬂer y damit er sich nur um die Y - Achse dreht
        Vector3 playerDirection = positionDiff;
        playerDirection.y = 0;
        playerDirection = playerDirection.normalized;

        // Zielrotation
        Quaternion lookRotation = Quaternion.LookRotation(playerDirection);

        // Smoothe Drehung
        transform.rotation = Quaternion.Slerp(
            transform.rotation,                     // Aktuelle rotation
            lookRotation,                           // Zielrotation
            Time.deltaTime * rotationSpeed          // drehgeschwindigkeit
        );

        if (positionDiff.magnitude > attackRange) // Zombie bewegt sich nur wenn der Spieler auﬂerhalb der damageRange ist
        {
            // Forw‰rtsbewegung
            Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
        }

    }
}
