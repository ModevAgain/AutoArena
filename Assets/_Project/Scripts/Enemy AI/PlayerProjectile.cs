using UnityEngine;
using System.Collections;

public class PlayerProjectile : BaseProjectile
{

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
           // other.GetComponentInParent<EnemyBehaviour>().Die();
            Destroy(this.gameObject);
        }

        base.OnTriggerEnter(other);
    }
}
