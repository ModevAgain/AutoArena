using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CraterFade : MonoBehaviour
{
    public MeshRenderer Crater;
    public MeshRenderer Ring;

    // Start is called before the first frame update
    public void Fade()
    {
        transform.parent = null;
        transform.DOMoveY(0.01f, 0.05f);

        Material mat = Crater.material;
        Color targetColor = mat.color;

        DOVirtual.Float(1, 0, 4, (f) =>
        {
            targetColor.a = f;
            mat.color = targetColor;
            mat.SetColor("_BaseColor", targetColor);
        });

                
        Material mat1 = Ring.material;
        Color targetColor1 = mat.color;

        DOVirtual.Float(1, 0, 4, (f) =>
        {
            targetColor1.a = f;
            mat1.color = targetColor1;
            mat1.SetColor("_BaseColor", targetColor1);
        }).OnComplete(() => gameObject.SetActive(false));


    }


}
