using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventCatcher : MonoBehaviour
{
    public Action AnimationEvent;
    
    public void TriggerAnimationEvent()
    { 
        AnimationEvent?.Invoke();
    }

}
