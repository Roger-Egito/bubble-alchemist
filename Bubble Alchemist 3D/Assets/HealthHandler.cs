using UnityEngine;
using UnityEngine.Events;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] private int life = 5;
    public UnityEvent OnDeath;

    public int currentLife;

    private void Start()
    {
        currentLife = life;
    }
    public void TakeDamage()
    {
        currentLife--;
        if(currentLife <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
