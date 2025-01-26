using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float velocity = 1f;
    [SerializeField] private float lifetime = 2;
    private Transform lastFamiliarPosition;
    private float currentTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastFamiliarPosition = GameObject.FindGameObjectWithTag("Familiar").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime >= lifetime)
        {
            Destroy(gameObject);
        }
        currentTime += Time.deltaTime;
        transform.position += (lastFamiliarPosition.position - transform.position).normalized * velocity * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Familiar"))
        {
            Destroy(gameObject);
        }
    }
}
