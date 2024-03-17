using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BearController : MonoBehaviour
{
    public float speed;
    public GameObject table;
    public List<GameObject> projectiles;
    public float engagementDistance = 4f; // Distance to engage the player

    private Animator animator;
    private Transform playerTransform;
    private bool isDying = false;

    public AudioSource bearAttack;
    public AudioSource bearDeath;
    public AudioSource backgroundMusic;

    private void Start()
    {
        backgroundMusic.Play();
        animator = GetComponent<Animator>();
        projectiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Projectile"));
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!isDying)
        {
            HandleMovementAndEngagement();
        }
        else
        {
            DisableMovementAndActions();
        }
    }

    private void HandleMovementAndEngagement()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > engagementDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            EngagePlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        transform.position += transform.forward * speed * Time.deltaTime;

        animator.SetBool("Run Forward", speed > 3f);
        animator.SetBool("WalkForward", speed <= 3f);
        animator.SetBool("Attack1", false);
    }

    private void EngagePlayer()
    {
        DisableMovementAndActions();
        animator.SetBool("Attack1", true);


        // Now proceed to disable everything
        foreach (var projectile in projectiles)
        {
            projectile.SetActive(false);
        }
        table.SetActive(false);
        ResetEnvironmentAndUI();

        bearAttack.Play();

        // Finally, start the coroutine to reload the scene after a delay
        StartCoroutine(ReloadSceneAfterDelay(5));
    }


    private void DisableMovementAndActions()
    {
        speed = 0;
        animator.SetBool("Run Forward", false);
        animator.SetBool("WalkForward", false);
    }

    private void ResetEnvironmentAndUI()
    {
        ScoreManager.Instance.ResetScore();
        UIManager.Instance.UpdateScoreUI();
        DisableDirectionalLight();
    }

    private void DisableDirectionalLight()
    {
        Light directionalLight = FindObjectOfType<Light>();
        if (directionalLight != null && directionalLight.type == LightType.Directional)
        {
            directionalLight.enabled = false;
        }
    }

    public void HitByProjectile()
    {
        if (!isDying)
        {
            isDying = true;
            StartCoroutine(PlayHitAndDeathAnimations());
            bearDeath.Play();
        }
    }

    private IEnumerator PlayHitAndDeathAnimations()
    {
        DisableMovementAndActions();
        animator.SetBool("Death", true);
        ScoreManager.Instance.AddScore(1);
        UIManager.Instance.UpdateScoreUI();
        yield return new WaitForSeconds(3.2f); // Wait for the animation
        Destroy(gameObject);
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
