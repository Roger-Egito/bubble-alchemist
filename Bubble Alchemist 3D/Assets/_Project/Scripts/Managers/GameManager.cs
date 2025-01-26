using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private int scoreIncrease = 5;
    [SerializeField] private int scoreThreshold = 20;

    [SerializeField] private UnityEvent<int> OnScoreChanged;
    [SerializeField] private UnityEvent<int> OnDifficulyChanged;

    [SerializeField] private int difficulty = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCurrentDifficulty()
    {
        return difficulty;
    }

    public void IncreaseScore()
    {
        score += scoreIncrease;
        if(score >= scoreThreshold)
        {
            scoreThreshold *= 2;
            difficulty++;
            OnDifficulyChanged?.Invoke(difficulty);
        }

        OnScoreChanged?.Invoke(score);
    }
}
