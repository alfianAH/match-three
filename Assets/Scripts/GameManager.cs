using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Instance 
    public static GameManager Instance;
    public Text scoreText,
        comboValue;
    private int playerScore,
        combo;

    public int Combo
    {
        get => combo;
        set
        {
            combo = value;
            comboValue.text = $"x{combo}";
        }
    }

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
        combo += 1;
        comboValue.text = $"x{combo}";
        
        playerScore += point * combo;
        scoreText.text = playerScore.ToString();
    }
}
