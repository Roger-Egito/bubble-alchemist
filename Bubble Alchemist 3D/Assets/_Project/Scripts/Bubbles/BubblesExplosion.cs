using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class BubblesExplosion : MonoBehaviour
{
    public int bubblesAmout = 0;
    [SerializeField] private int bubbeThreshold = 5;
    [SerializeField] private ParticleSystem ps;

    [SerializeField] private UnityEvent OnExplode;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.tag);
        if (collision.collider.CompareTag("Bubble"))
        {
            Debug.Log("simmm");
            bubblesAmout++;
            if (bubblesAmout > bubbeThreshold)
            {
                bubblesAmout = 0;
                ps.Play();
                OnExplode.Invoke();
            }
        }  
    }
}
