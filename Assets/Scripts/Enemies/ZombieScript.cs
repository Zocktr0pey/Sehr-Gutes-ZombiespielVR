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

    private GameObject player;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Zombie rotiert zum Spieler
        // *Redacted* simpler aber keine Smoothness
        // Vector3 playerPos = player.transform.position;
        // Eigene y position damit der zombie nur entlang der Y achse rotiert
        // transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));

        // Richtungsvektor zum Spieler, außer y damit er sich nur um die Y-Achse dreht
        Vector3 playerDirection = (player.transform.position - transform.position);
        playerDirection.y = 0;
        playerDirection = playerDirection.normalized;

        // Zielrotation
        Quaternion lookRotation = Quaternion.LookRotation(playerDirection);

        // Smoother übergang
        transform.rotation = Quaternion.Slerp(
            transform.rotation,                     // current rotation
            lookRotation,                           // target rotation
            Time.deltaTime * rotationSpeed          // rotation speed
        );

        Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);

    }
}
