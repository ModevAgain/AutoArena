using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public CinemachineBrain Brain;
    public CinemachineVirtualCamera FallingCam;
    public CinemachineVirtualCamera GameCam;
    public CinemachineVirtualCamera DiceCam;

    public CinemachineDollyCart Cart_DiceCam;

    private CinemachineBasicMultiChannelPerlin _noiseComponentGame;
    private CinemachineBasicMultiChannelPerlin _noiseComponentFalling;

    private void Start()
    {
        _noiseComponentGame = GameCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _noiseComponentFalling = FallingCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void OnFinishedFalling()
    {
        DOVirtual.DelayedCall(0.5f, () => {
            GameCam.enabled = true;
            FallingCam.enabled = false;
        });
    }

    public void ShakeFallingCamManual(float value)
    {
        _noiseComponentFalling.m_FrequencyGain = value;
        _noiseComponentFalling.m_AmplitudeGain = value;
    }

    public void ShakeCamera()
    {
        _noiseComponentGame.m_FrequencyGain = 1;
        _noiseComponentGame.m_AmplitudeGain = 1;

        

        DOVirtual.DelayedCall(0.3f, () =>
        {
            _noiseComponentGame.m_FrequencyGain = 0;
            _noiseComponentGame.m_AmplitudeGain = 0;
        });
    }

    public void ToggleGameCam()
    {
        GameCam.enabled = true;
        DiceCam.enabled = false;
        FallingCam.enabled = false;
    }

    public void ToggleDiceCamera()
    {
        DiceCam.enabled = true;
        GameCam.enabled = false;
        FallingCam.enabled = false;
    }

    public void DriveDiveCamera(System.Action driveFinished)
    {
        DOVirtual.Float(0, 1, 3, (f) => Cart_DiceCam.m_Position = f).SetDelay(1).SetEase(Ease.InOutSine).OnComplete(() => driveFinished?.Invoke());        
    }

    public void ResetDiceDiveCam()
    {        
        Cart_DiceCam.m_Position = 0;
    }
}
