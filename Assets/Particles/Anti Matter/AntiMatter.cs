using System;
using UnityEngine;

public class AntiMatter : Particle
{
    AntiMatterStats antiMatterStats;
    public static event Action OnAntiMatterCreated; // Event for mass change

    protected override void Start()
    {
        base.Start();
        antiMatterStats = stats as AntiMatterStats;
        OnAntiMatterCreated?.Invoke();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out AntiMatter antiMatter))
        {
            if (antiMatter.fusions.Equals(fusions))
            {
                Fuse(other.gameObject);
            }
        }

        if (other.gameObject.TryGetComponent(out Matter matter))
        {
            if (matter.invincible) return;

            if (matter.fusions < fusions + 1)
            {
                Destroy(matter.gameObject);
                Die(matter);
            }
            else
            {
                matter.Fission(fusions + 1);
                Die(matter);
            }
        }
    }

    void Die(Matter matter)
    {
        SoundManager.Instance.PlayRandomSound(antiMatterStats.destroySounds, transform, UnityEngine.Random.Range(1 / (matter.fusions + 1), 1));
        ScreenShake.Instance.Shake(.1f, 0.1f);
        Handheld.Vibrate();

        //OnFusionsChanged?.Invoke(fusions);

        Instantiate(antiMatterStats.destroyMatter, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
