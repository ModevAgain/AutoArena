using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerProjectile : BaseProjectile
{

    public Transform Particles;
    public ParticleSystem ImpactParticles;
    public Light ParticleLight;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponentInParent<EnemyBehaviour>().GetDamage(_dmg, other.transform.position - transform.position);
        }

        Particles.parent = null;

        ImpactParticles.Play();
        ParticleLight.DOIntensity(0, 0.3f);
        DOVirtual.DelayedCall(0.6f, () => Destroy(Particles.gameObject));

        base.OnTriggerEnter(other);
    }
}
