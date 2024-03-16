using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int currentScore = 0;
    public int CurrentScore 
    {
        get { return currentScore; }
        private set { currentScore = value; }
    }

    public int HighScore { get; private set; } = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        CurrentScore += scoreToAdd;
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore; 
        }
    }

    public void ResetScore()
    {
        CurrentScore = 0; 
    }
}
