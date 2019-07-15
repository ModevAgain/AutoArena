using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Data")]
    public float Health;

    [Header("Stats")]
    public Stats PlayerStats;


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

    public void ReapplyChangedStats(Stats s)
    {
        PlayerStats = s;

        //Apply to subsystems as well, e.g. agent for movespeed
    }

    [System.Serializable]
    public struct Stats
    {
        public float MaxHealth;
        public float MoveSpeed;
        public float AttackSpeed;
        public float Damage;
    }
}
