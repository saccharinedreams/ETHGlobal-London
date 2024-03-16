using UnityEngine;
using TMPro; 

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI currentScoreText; 
    public TextMeshProUGUI highScoreText; 

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

    void Start()
    {
        UpdateScoreUI(); 
    }

    public void UpdateScoreUI()
    {
        if (ScoreManager.Instance != null)
        {
            currentScoreText.text = "Score: " + ScoreManager.Instance.CurrentScore.ToString();
            highScoreText.text = "High Score: " + ScoreManager.Instance.HighScore.ToString();
        }
    }
}
