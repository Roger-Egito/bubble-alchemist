using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject stone;
    [SerializeField] private float spawnSize;
    
    public void Spawn()
    {
        Instantiate(stone, transform.position + Vector3.right * Random.Range(-spawnSize, spawnSize), Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.left * spawnSize, transform.position + Vector3.right * spawnSize);
    }
}
