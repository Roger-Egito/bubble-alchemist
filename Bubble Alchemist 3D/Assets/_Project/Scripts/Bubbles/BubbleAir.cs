using UnityEngine;

public class BubbleAir : Bubble
{
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float explosionForce = 1f;
    [SerializeField] private LayerMask itemMask;
    public override void Pop()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, itemMask);
        foreach(Collider2D col in hits)
        {
            col.attachedRigidbody.AddForce((col.transform.position - transform.position) * explosionForce, ForceMode2D.Impulse);
        }

        base.Pop();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
