using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private WaveManager _waveMan;
    private DiceManager _diceMan;
    private CameraManager _camMan;
    private PlayerData _playerData;


    private void Awake()
    {
        _waveMan = FindObjectOfType<WaveManager>();
        _diceMan = FindObjectOfType<DiceManager>();
        _camMan = FindObjectOfType<CameraManager>();
        _playerData= FindObjectOfType<PlayerData>();
    }

    // Start is called before the first frame update
    void Start()
    {


        _waveMan.AllWavesFinished = () => StartCoroutine(StartItemIntermezzo());
        _diceMan.FinishedDicing = (stats) => 
        {
            StartCoroutine(EndItemIntermezzo());
            _playerData.ReapplyChangedStats(stats);
        };
    }

    public IEnumerator StartItemIntermezzo()
    {
        yield return new WaitForSeconds(2);
        _camMan.ToggleDiceCamera();
        _diceMan.SetStats(_playerData.PlayerStats);
        yield return new WaitForSeconds(_camMan.Brain.m_DefaultBlend.m_Time);
        _diceMan.enabled = true;
    }

    public IEnumerator EndItemIntermezzo()
    {
        yield return new WaitForSeconds(2);
        _camMan.ToggleGameCam();
        yield return new WaitForSeconds(_camMan.Brain.m_DefaultBlend.m_Time);
        _diceMan.ResetDice();
        _diceMan.enabled = false;
        _waveMan.SpawnNextWave();
    }
}
