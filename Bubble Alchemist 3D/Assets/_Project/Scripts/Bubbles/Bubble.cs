using UnityEngine;
using UnityEngine.Rendering;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private Vector2 velocityRandomScale = Vector2.one;

    [Range(0f, 1f)]
    [SerializeField] private float rarityLevel = 0.5f;

    public float RarityLevel { get { return rarityLevel; }}
   
    private float velocity = 1f;

    public float Velocity { get; set; }

    private float currentTime = 0f;
    public virtual void Start()
    {
        velocity = Random.Range(velocityRandomScale.x, velocityRandomScale.y);
    }

    void Update()
    {
        if (currentTime > lifeTime)
        {
            gameObject.SetActive(false);
            currentTime = 0f;
        }
        currentTime += Time.deltaTime;
        transform.position += Vector3.up * velocity * Time.deltaTime;
    }

    public virtual void Pop()
    {
        gameObject.SetActive(false);
    }
}
