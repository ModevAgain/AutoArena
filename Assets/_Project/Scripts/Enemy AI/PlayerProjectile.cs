using UnityEngine;
using System.Collections;

public class PlayerProjectile : BaseProjectile
{

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponentInParent<EnemyBehaviour>().GetDamage(_dmg);
            Destroy(this.gameObject);
        }

        base.OnTriggerEnter(other);
    }
}
