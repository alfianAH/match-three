using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Instance 
    public static GameManager Instance;
    public Text scoreText;
    private int playerScore;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int point)
    {
        playerScore += point;
        scoreText.text = playerScore.ToString();
    }
}
