using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{

    public CinemachineVirtualCamera FallingCam;
    public CinemachineVirtualCamera GameCam;

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
}
