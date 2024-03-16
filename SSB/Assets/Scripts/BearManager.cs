using UnityEngine;

public class BearManager : MonoBehaviour
{
    public GameObject bearPrefab;
    public Vector3 spawnPoint;
    public float initialSpawnDelay = 2f; // Set to spawn the first bear after 2 seconds
    public float spawnInterval = 10f;
    public float decreaseAmount = 1f;
    public float minimumInterval = 1f;
    private float timeSinceLastSpawn;
    private float elapsedTime = 0f;
    private bool firstSpawn = true;

    private void Start()
    {
        timeSinceLastSpawn = -initialSpawnDelay; // Start counting from negative initial delay for the first spawn
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        // Check if it's time to spawn a new bear
        if (timeSinceLastSpawn >= spawnInterval || firstSpawn)
        {
            SpawnBear(elapsedTime);
            timeSinceLastSpawn = 0f; // Reset the timer
            spawnInterval = Mathf.Max(minimumInterval, spawnInterval - decreaseAmount); // Decrease the spawn interval

            if (firstSpawn) firstSpawn = false; // Ensure the first spawn logic only runs once
        }
    }

    private void SpawnBear(float elapsedTime)
    {
        GameObject newBear = Instantiate(bearPrefab, spawnPoint, Quaternion.identity);
        var bearController = newBear.GetComponent<BearController>();

        // Increase the speed based on elapsed time
        bearController.speed = Mathf.Log(elapsedTime + 1) / 3 + 3;
    }
}
