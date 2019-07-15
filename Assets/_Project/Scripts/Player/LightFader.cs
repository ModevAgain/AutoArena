using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFader : MonoBehaviour
{
    public AnimationCurve LightCurve;

    private float _startIntensity;
    private Light _light;
    private float startY;
    private CameraManager _camMan;

    private void Awake()
    {
        startY = transform.position.y;
        _light = GetComponent<Light>();
        _startIntensity = _light.intensity;
        _camMan = FindObjectOfType<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
         _light.intensity =  _startIntensity *  LightCurve.Evaluate(1 - (transform.position.y / startY));
        _camMan.ShakeFallingCamManual(LightCurve.Evaluate(1 - (transform.position.y / startY)));

    }
}
