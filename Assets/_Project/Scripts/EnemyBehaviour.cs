using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public MeshRenderer AttackedMeshrenderer;
    private EnemyManager _enemyMan;

    private void Awake()
    {
        _enemyMan = FindObjectOfType<EnemyManager>();
    }

    public void IsBeingAttacked(bool isBeingAttacked)
    {
        AttackedMeshrenderer.enabled = isBeingAttacked;
    }

    public void Die()
    {
        _enemyMan.UnregisterEnemy(this);
        Destroy(this.gameObject);
    }
}
