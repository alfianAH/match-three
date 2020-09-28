using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Instance 
    public static GameManager instance;
    public Text scoreText;
    private int playerScore;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        } 
        else if (instance != null)
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
