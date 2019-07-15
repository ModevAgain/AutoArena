using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceData", menuName = "Data/DiceData")]
public class DiceData : ScriptableObject
{
    
    public Face[] Faces;
    public Vector2 TextureOffset;

    [System.Serializable]
    public struct Face
    {
        public FaceDirections Direction;
        public ItemData Item;
    }

    public enum FaceDirections
    {
        FORWARD,
        UP,
        DOWN,
        BACK,
        RIGHT,
        LEFT
    }
}
