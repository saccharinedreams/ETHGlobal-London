using UnityEngine;

public class BearController : MonoBehaviour
{
    public float speed;
    private Animator animator;
    private Transform playerTransform; // To keep track of the player's position
    public GameObject table;
    public GameObject projectile;
    private bool tableDisabled = false;
    private bool projectileDisabled = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has a "Player" tag
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > 1.5)
        {
            // Bear moves towards the player if farther than 1 unit from the player
            transform.position += transform.forward * speed * Time.deltaTime;
            
            // Check if the bear should run or walk based on its speed
            if (speed > 4f)
            {
                animator.SetBool("Run Forward", true);
                animator.SetBool("WalkForward", false);
            }
            else
            {
                animator.SetBool("Run Forward", false);
                animator.SetBool("WalkForward", true);
            }
            animator.SetBool("Attack1", false); // Ensure the attack animation is not played
        }
        else
        {
            Debug.Log(distanceToPlayer);
            // Bear stops and attacks if within 1 unit of the player
            speed = 0; // Stop the bear's movement
            animator.SetBool("Run Forward", false);
            animator.SetBool("WalkForward", false); // Ensure walking animation is not played
            animator.SetBool("Attack1", true);
            if (!tableDisabled)
            {
                table.SetActive(false);
                tableDisabled = true; // Ensure this happens only once
            }

            if (!projectileDisabled)
            {
                projectile.SetActive(false);
                projectileDisabled = true; // Ensure this happens only once
            }
        }
    }
}
