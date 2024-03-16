using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


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
            if (distanceToPlayer > 2)
            {
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                // Ensure the bear is always facing the player as it moves
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                
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
                projectile.SetActive(false);
                table.SetActive(false);
                ScoreManager.Instance.ResetScore();
                UIManager.Instance.UpdateScoreUI();
                Light directionalLight = FindObjectOfType<Light>();
                if (directionalLight != null && directionalLight.type == LightType.Directional)
                {
                    directionalLight.enabled = false;
                }
                StartCoroutine(ReloadSceneAfterDelay(5)); // Wait for 5 seconds before resetting the scene
            }
        }   
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

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
        ScoreManager.Instance.AddScore(1); 
        UIManager.Instance.UpdateScoreUI();
        yield return new WaitForSeconds(3.2f); // Wait for the death animation to play through
        Destroy(gameObject); // Remove the bear after the animations
    }
}
