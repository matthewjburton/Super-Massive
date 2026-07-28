using UnityEngine;

public class MenuMatter : Particle
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out MenuMatter menuMatter))
        {
            if (menuMatter.fusions.Equals(fusions))
            {
                Fuse(other.gameObject);
            }
        }
    }

    protected override float GetSpeed()
    {
        return stats.defaultSpeed / (fusions + 1);
    }
}
