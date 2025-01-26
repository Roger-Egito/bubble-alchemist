using UnityEngine;

public class BubbleEarth : Bubble
{
    [SerializeField] private StoneSpawner stoneSpawner;

    public override void Start()
    {
        stoneSpawner = GameObject.FindAnyObjectByType<StoneSpawner>();
        base.Start();
    }

    public override void Pop()
    {
        stoneSpawner.Spawn();
        base.Pop();
    }
}
