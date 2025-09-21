using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieScript : NetworkBehaviour
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
    private bool onCooldown;

    private GameObject[] players;
    private GameObject nearestPlayer;
    private PlayerScript playerScript;
    private Rigidbody rb;
    Vector3 positionDiff;
    Vector3 playerDirection;

    private NetworkShit networkShit;

    private void Awake()
    {
        networkShit = NetworkShit.Instance;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        onCooldown = false;
    }

    /*
    public override void OnNetworkSpawn()
    {
        networkShit = NetworkShit.Instance;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        onCooldown = false;
    }
    */

    // Update is called once per frame
    void Update()
    {
        ReduceCooldown();

        players = networkShit.GetPlayers();

        nearestPlayer = GetNearestPlayer();

        if (nearestPlayer == null )
        {
            return;
        }

        positionDiff = (nearestPlayer.transform.position - transform.position);

        if (positionDiff.magnitude <= attackRange && !onCooldown)
        {
            Attack();
        }

        Rotate();

        if (positionDiff.magnitude > attackRange) // Zombie bewegt sich nur wenn der Spieler auﬂerhalb der damageRange ist
        {
            Move();
        }
    }

    private GameObject GetNearestPlayer()
    {
        GameObject nearestPlayer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject player in networkShit.GetPlayers())
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPlayer = player;
            }
        }

        return nearestPlayer;
    }

    private void Attack()
    {
        nearestPlayer.GetComponent<PlayerScript>().TakeDamage(damage);
        onCooldown = true;
    }

    private void Move()
    {
        // Forw‰rtsbewegung
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
        // Richtungsvektor zum Spieler auﬂer y damit er sich nur um die Y - Achse dreht
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
}
