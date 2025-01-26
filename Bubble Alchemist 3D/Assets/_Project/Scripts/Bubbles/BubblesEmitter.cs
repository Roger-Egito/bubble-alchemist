using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BubblesEmitter : MonoBehaviour
{
    [SerializeField] private GameObject[] bubbleTypes;
    [SerializeField] private BubblePool pool;

    [SerializeField] private float spawnSize;

    [SerializeField] private float spawnTime;

    private float currentTime = 0f;

    private void Start()
    {
        pool = GameObject.FindAnyObjectByType<BubblePool>();
    }

    void Update()
    {
        if(currentTime >= spawnTime)
        {
            var b = pool.GetRandomBubble();
            if(b != null)
                b.transform.position = transform.position + Vector3.right * Random.Range(-spawnSize, spawnSize);
            currentTime = 0;
        }
        currentTime += Time.deltaTime;
    }

    public void UpdateDifficultu(int newDiff)
    {
        spawnTime /= newDiff;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.left * spawnSize, transform.position + Vector3.right * spawnSize);
    }
}
