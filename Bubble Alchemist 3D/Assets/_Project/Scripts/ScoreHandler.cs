using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    
    public void UpdateScore(int score)
    {
        textMesh.text = "Score: " + score.ToString();
    }
}
