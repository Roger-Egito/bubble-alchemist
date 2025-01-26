using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BubblePool : MonoBehaviour
{
    [SerializeField] private int maxBubbles = 10;
    [SerializeField] private GameObject[] bubbles;
    [SerializeField] private List<Bubble> pooledObjects;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int j = 0; j < bubbles.Length; j++) 
        {
            var bubble =  bubbles[j].GetComponent<Bubble>();

            for (int i = 0; i < maxBubbles / bubbles.Length; i++)
            {

                var bubbleObj = Instantiate(bubbles[j], transform);
                bubbleObj.SetActive(false);
                pooledObjects.Add(bubbleObj.GetComponent<Bubble>());
            }
        }
    }

    public Bubble GetRandomBubble()
    {
        float rarity = Random.value;
        List<Bubble> possibleBubbles = new List<Bubble>();

        foreach (var bubble in pooledObjects)
        {
            if (!bubble.isActiveAndEnabled)
            {
                if (rarity <= bubble.RarityLevel)
                {
                    possibleBubbles.Add(bubble);
                }
            }
        }

        if(possibleBubbles.Count > 0)
        {
            var b = possibleBubbles[Random.Range(0, possibleBubbles.Count)];
            b.gameObject.SetActive(true);
            return b;
        }

        return null;
    }
}
