using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class DiceManager : MonoBehaviour
{
    [Header("References")]
    public List<Transform> DiceSpawns;
    public GameObject DiceObject;
    public DiceData[] DiceData;
    public Transform SpinTarget;
    public TextMeshPro Text;
    public Action<PlayerData.Stats> FinishedDicing;
    public StatsVisualManager _statsMan;

    [SerializeField]
    private List<DiceScript> _spawnedDice;
    private CameraManager _camMan;

    private Sequence _highlightSeq;
    private int _highlightedDiceIndex = -1;
    private bool _diceWasThrown;
    private PlayerData.Stats _playerStats;

    // Start is called before the first frame update
    void Awake()
    {
        _spawnedDice = new List<DiceScript>();
        _camMan = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {

        if (!_diceWasThrown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _diceWasThrown = true;
                SpawnDice();
            }
        }
    }


    public void SpawnDice()
    {
        _camMan.DriveDiveCamera(ShowStats);
        Text.DOFade(0, 0.2f);

        ResetDice();

        _spawnedDice = new List<DiceScript>();

        int index = 0;
        foreach (var item in DiceSpawns)
        {
            DiceScript temp = Instantiate(DiceObject).GetComponent<DiceScript>();
            temp.transform.position = item.position;
            _spawnedDice.Add(temp);
            temp.Index = index;
            temp.Setup(DiceData[UnityEngine.Random.Range(0, 3)]);

            index++;
        }

        foreach (var item in _spawnedDice)
        {
            item.Throw(SpinTarget, this);
        }

    }

    public void SetStats(PlayerData.Stats stats)
    {
        _playerStats = stats;
    }

    public void ShowStats()
    {
        _statsMan.SetStats(_playerStats);
        _statsMan.ShowStats();
    }

    public void ResetDice()
    {
        foreach (var item in _spawnedDice)
        {
            Destroy(item.gameObject);
        }

        Text.DOFade(1, 0);
        _statsMan.HideStats();
    }

    public void HighlightDice(int diceIndex)
    {
        if (_highlightedDiceIndex == diceIndex)
        {
            ActivateDice();
            return;
        }

        _highlightedDiceIndex = diceIndex;

        _highlightSeq.Kill();
        _highlightSeq = DOTween.Sequence();

        foreach (var item in _spawnedDice)
        {
            if(item.Index == diceIndex)
            {
                _highlightSeq.Insert(0,item.Highlight());
                ItemData data = item.GetItem();
                _statsMan.ShowBonus(data.Type, data.Value);

            }
            else
            {
                _highlightSeq.Insert(0, item.Dehighlight());
            }
        }
        
    }

    public void ActivateDice()
    {
        Debug.Log(_spawnedDice[_highlightedDiceIndex].GetItem());

        ItemData data = _spawnedDice[_highlightedDiceIndex].GetItem();

        if(data.Type == ItemData.ItemType.ATTACKSPEED)
        {
            _playerStats.AttackSpeed += data.Value;
        }
        else if (data.Type == ItemData.ItemType.MOVESPEED)
        {
            _playerStats.MoveSpeed += data.Value;
        }
        else if (data.Type == ItemData.ItemType.HEALTH)
        {
            _playerStats.MaxHealth += data.Value;
        }
        else if (data.Type == ItemData.ItemType.DAMAGE)
        {
            _playerStats.Damage += data.Value;
        }

        _statsMan.ApplyBonus(_playerStats);

        FinishedDicing?.Invoke(_playerStats);
    }



}
