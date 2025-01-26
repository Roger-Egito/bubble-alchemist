using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] private float velocity = 1f;
    [SerializeField] private float lifetime = 3f;

    private float currentTime = 0f;
    // Update is called once per frame
    void Update()
    {
        if (currentTime >= lifetime) Destroy(gameObject);
        transform.position += Vector3.down * velocity * Time.deltaTime;
        currentTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Familiar"))
            Debug.Log("Acertou");
    }
}
