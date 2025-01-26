using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    public string texto = "Score:";
    public void UpdateScore(int score)
    {
        textMesh.text = texto + score.ToString();
    }
}
