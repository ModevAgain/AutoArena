using UnityEngine;
using System.Collections;

public class MarineProjectile : BaseProjectile
{

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //other.GetComponent<EnemyBehaviour>().Die();
            Destroy(this.gameObject);
        }

        base.OnTriggerEnter(other);
    }
}
