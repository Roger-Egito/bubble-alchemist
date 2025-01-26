using UnityEngine;
using UnityEngine.Events;

public class BubblesExplosion : MonoBehaviour
{
    private int bubblesAmout = 0;
    [SerializeField] private int bubbeThreshold = 5;

    [SerializeField] private UnityEvent OnExplode;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bubble"))
        {
            bubblesAmout++;
            if(bubblesAmout > bubbeThreshold)
            {
                bubblesAmout = 0;
                OnExplode.Invoke();
            }
        }
    }
}
