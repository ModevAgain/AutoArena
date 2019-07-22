using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DebugCanvas : MonoBehaviour
{
    public UIRaycastCatcher Catcher;

    private int _maxTapCount = 4;
    private int _tapped;

    private float _maxTapTime = 2;
    private float _tapTimer;

    private float _activeMaxTimer = 4;
    private float _activeTimer;

    private CanvasGroup _CG;

    private bool _active;


    private float current;
    public TextMeshProUGUI FPSText;

    // Start is called before the first frame update
    void Start()
    {
        Catcher = GetComponentInChildren<UIRaycastCatcher>();
        Catcher.OnPointerDown = OnCatcherDown;

        _CG = GetComponent<CanvasGroup>();

        HideUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(_tapped != 0)
        {
            _tapTimer += Time.deltaTime;

            if (_tapTimer >= _maxTapTime)
                _tapped = 0;
        }

        if (_active)
        {
            _activeTimer += Time.deltaTime;

            if (_activeTimer >= _activeMaxTimer)
            {
                _activeTimer = 0;
                HideUI();
            }
        }


        current = Time.frameCount / Time.time;
        FPSText.text = "" + (int)current;

    }

    private void OnCatcherDown()
    {
        _tapped++;

        _tapTimer = 0;

        if (_tapped >= _maxTapCount)
            ShowUI();
    }

    private void ShowUI()
    {
        _CG.DOFade(1, 0.2f);
        Catcher.Image.raycastTarget = false;
        _activeTimer = 0;

        _active = true;

    }

    private void HideUI()
    {
        _active = false;

        _CG.DOFade(0, 0.2f);
        Catcher.Image.raycastTarget = true;
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
