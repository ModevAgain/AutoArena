using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIRaycastCatcher : MonoBehaviour, IPointerDownHandler
{
    public Action OnPointerDown;
    public Image Image;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnPointerDown?.Invoke();
    }
}
