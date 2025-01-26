using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BubblesEmitter : MonoBehaviour
{
    [SerializeField] private GameObject[] bubbleTypes;

    [SerializeField] private float spawnSize;

    [SerializeField] private float spawnTime;

    private float currentTime = 0f;


    void Update()
    {
        if(currentTime >= spawnTime)
        {
            Instantiate(bubbleTypes[Random.Range(0, bubbleTypes.Length)], transform.position + Vector3.right * Random.Range(-spawnSize, spawnSize), Quaternion.identity);
            currentTime = 0;
        }
        currentTime += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.left * spawnSize, transform.position + Vector3.right * spawnSize);
    }
}
