using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Data")]
    public float Health;
    private CameraManager _camManager;

    // Start is called before the first frame update
    void Start()
    {

        _camManager = FindObjectOfType<CameraManager>();
    }

    
    public void GetDamage(float dmg)
    {
        Health = Mathf.Clamp(Health - dmg, 0, 10000);

        _camManager.ShakeCamera();

        //HealthBar.DOFillAmount(Health / 100, 0.15f);

        //if (Health <= 0)
        //    Die();
    }
}
