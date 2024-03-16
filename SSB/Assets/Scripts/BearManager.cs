using UnityEngine;

public class BearManager : MonoBehaviour
{
    public GameObject bearPrefab;
    public Vector3[] spawnPoints; // Array of spawn points
    public float initialSpawnDelay = 2f;
    public float spawnInterval = 10f;
    public float decreaseAmount = 0.4f;
    public float minimumInterval = 3f;
    private float timeSinceLastSpawn;
    private float elapsedTime = 0f;
    private bool firstSpawn = true;
    public float initialSpawnChance = 0.4f; // chance to spawn at start
    public float spawnChanceIncrease = 0.05f;
    public float spawnChanceIncrementInterval = 10f;
    private float spawnChance;

    private void Start()
    {
        timeSinceLastSpawn = -initialSpawnDelay;
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval || firstSpawn)
        {
            spawnChance = Mathf.Clamp(initialSpawnChance + (spawnChanceIncrease * elapsedTime / spawnChanceIncrementInterval), 0f, 1f);
            foreach (var spawnPoint in spawnPoints)
            {
                // Perform random check for each spawn point
                if (Random.value <= spawnChance) SpawnBear(spawnPoint, elapsedTime);
            }
            timeSinceLastSpawn = 0f;
            spawnInterval = Mathf.Max(minimumInterval, spawnInterval - decreaseAmount);

            if (firstSpawn) firstSpawn = false;
        }
    }

    private void SpawnBear(Vector3 spawnPoint, float elapsedTime)
    {

        GameObject newBear = Instantiate(bearPrefab, spawnPoint, Quaternion.identity);
        var bearController = newBear.GetComponent<BearController>();
        bearController.speed = Mathf.Log(elapsedTime + 1f) / 2f + 1.5f;
    }
}
