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

    public void IncreaseScore(int news)
    {
        int a = (int)difficulty / 2;
        if (a <= 0) a = 1;
        score += news * a;
        if(score >= scoreThreshold)
        {
            scoreThreshold *= 2;
            difficulty++;
            OnDifficulyChanged?.Invoke(difficulty);
        }

        OnScoreChanged?.Invoke(score);
    }
}
