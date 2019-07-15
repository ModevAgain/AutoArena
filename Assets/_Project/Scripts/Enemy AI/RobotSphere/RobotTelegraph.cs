using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTelegraph : MonoBehaviour
{
    public Material Mat;

    private void Awake()
    {
        Mat = GetComponentInChildren<MeshRenderer>().material;
    }
}
