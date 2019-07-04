using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEventCatcher : MonoBehaviour
{
    public Action<Collider> TriggerEnter;
  

    public void OnTriggerEnter(Collider other)
    {

        TriggerEnter?.Invoke(other);
    }
}
