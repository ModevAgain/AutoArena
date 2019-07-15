using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MarineProjectile : BaseProjectile
{
    public Light MuzzleLight;

    private void Start()
    {        
        DOVirtual.DelayedCall(0.0f, (() =>
        {

            if (MuzzleLight != null)
                MuzzleLight.enabled = false;
        }));
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerData>().GetDamage(_dmg);    
            if(MuzzleLight != null)
                Destroy(MuzzleLight.transform.parent.gameObject);
        }

        base.OnTriggerEnter(other);
    }
}
