using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class DiceScript : MonoBehaviour
{
    public DiceData Data;
    public int Index;

    public float ThrowSpeed;
    private Rigidbody _rigid;
    private DiceManager _diceMan;
    private float _startScale;
    private bool _highlighted;

    // Start is called before the first frame update
    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        transform.rotation = Random.rotation;
        _startScale = transform.localScale.x;
    }

    public void Setup(DiceData data)
    {
        Data = data;
        GetComponent<MeshRenderer>().material.SetTextureOffset("_BaseMap", Data.TextureOffset);
        

    }

    public void Throw(Transform spinTarget, DiceManager diceMan)
    {
        _diceMan = diceMan;

        _rigid.isKinematic = false;

        _rigid.AddTorque(Random.rotation.eulerAngles * 10000, ForceMode.Impulse);
        _rigid.AddForce((spinTarget.position - transform.position).normalized * ThrowSpeed, ForceMode.Impulse);
    }

    public Sequence Highlight()
    {
        Sequence seq = DOTween.Sequence();

        _highlighted = true;

        _rigid.constraints = RigidbodyConstraints.FreezeRotation;

        seq.Insert(0, transform.DOLocalMoveY(transform.position.y + 0.5f, 0.2f));
        seq.Insert(0, transform.DOScale(_startScale * 1.5f, 0.2f));

        return seq;
    }

    public Sequence Dehighlight()
    {
        Sequence seq = DOTween.Sequence();

        if (_highlighted)
        {
            seq.Insert(0, transform.DOScale(_startScale, 0.2f));
            _highlighted = false;
        }

        return seq;
    }

    public void OnMouseDown()
    {
        _diceMan.HighlightDice(Index);
    }

    public ItemData GetItem()
    {
        if(Vector3.Dot(transform.up, Vector3.up) >= 0.95f)
        {
            return Data.Faces.ToList().Where((f) => f.Direction == DiceData.FaceDirections.UP).First().Item; 
        }
        else if (Vector3.Dot(-transform.up, Vector3.up) >= 0.95f)
        {
            return Data.Faces.ToList().Where((f) => f.Direction == DiceData.FaceDirections.DOWN).First().Item;
        }
        else if (Vector3.Dot(transform.forward, Vector3.up) >= 0.95f)
        {
            return Data.Faces.ToList().Where((f) => f.Direction == DiceData.FaceDirections.FORWARD).First().Item;
        }
        else if (Vector3.Dot(-transform.forward, Vector3.up) >= 0.95f)
        {
            return Data.Faces.ToList().Where((f) => f.Direction == DiceData.FaceDirections.BACK).First().Item;
        }
        else if (Vector3.Dot(transform.right, Vector3.up) >= 0.95f)
        {
            return Data.Faces.ToList().Where((f) => f.Direction == DiceData.FaceDirections.RIGHT).First().Item;
        }
        else if (Vector3.Dot(-transform.right, Vector3.up) >= 0.95f)
        {
            return Data.Faces.ToList().Where((f) => f.Direction == DiceData.FaceDirections.LEFT).First().Item;
        }
        return null;
    }
}
