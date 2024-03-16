using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour
{
    public float speed;
    private Animator animator;
    private Transform playerTransform; // To keep track of the player's position
    public GameObject table;
    public GameObject projectile;
    private bool tableDisabled = false;
    private bool projectileDisabled = false;
    private bool isDying = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has a "Player" tag
    }

    void Update()
    {
        if(isDying){
            speed = 0;
            animator.SetBool("Run Forward", false);
            animator.SetBool("WalkForward", false);
            animator.SetBool("Attack1", false);
        }
        else {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer > 1.5)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
                
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
                animator.SetBool("Attack1", false);
            }
            else
            {
                speed = 0; // Stop the bear's movement
                animator.SetBool("Run Forward", false);
                animator.SetBool("WalkForward", false);
                animator.SetBool("Attack1", true);
                // if (!tableDisabled)
                // {
                //     table.SetActive(false);
                //     tableDisabled = true;
                // }

                // if (!projectileDisabled)
                // {
                //     projectile.SetActive(false);
                //     projectileDisabled = true;
                // }
            }
        }   
    }

    // Add this method to handle being hit by a projectile
    public void HitByProjectile()
    {
        if(!isDying){
            Debug.Log("HitByProjectile called and isDying was false.");
            isDying = true;
            StartCoroutine(PlayHitAndDeathAnimations());
        }
    }

    private IEnumerator PlayHitAndDeathAnimations()
    {
        // Ensure the bear stops moving and attacking
        Debug.Log("PlayHitAndDeathAnimations coroutine started.");
        speed = 0;
        animator.SetBool("Run Forward", false);
        animator.SetBool("WalkForward", false);
        animator.SetBool("Attack1", false);

        // Play "Death" animation
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(3.2f); // Wait for the death animation to play through
        Destroy(gameObject); // Remove the bear after the animations
    }
}
