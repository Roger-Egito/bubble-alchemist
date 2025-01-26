using UnityEngine;

public class BubbleFire : Bubble
{
    [SerializeField] private GameObject fireBall;

    public override void Pop()
    {
        Instantiate(fireBall, transform.position, Quaternion.identity);

        base.Pop();
    }
}
