using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene("GameLayout6");
    }
}
