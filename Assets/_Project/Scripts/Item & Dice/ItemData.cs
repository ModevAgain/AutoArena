using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite Image;

    public ItemType Type;
    public float Value;

    public enum ItemType
    {
        ATTACKSPEED,
        MOVESPEED,
        DAMAGE,
        HEALTH,
        NULL
    }
}


