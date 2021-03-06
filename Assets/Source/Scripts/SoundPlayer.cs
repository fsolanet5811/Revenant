using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    
    private bool _wasAlerted;

    [SerializeField]
    private AudioSource _backgroundAudioSource;

    [SerializeField]
    private AudioSource _alertAudioSource;

    void Update()
    {
        bool alert = EnemiesManager.Instance.IsEnemyAlerted();

        if (alert && !_wasAlerted && _alertAudioSource != null)
            _alertAudioSource.Play();

        _backgroundAudioSource.pitch = alert ? 2 : 1;
        _backgroundAudioSource.volume = alert ? 1 : 0.5f;

        _wasAlerted = alert;
    }
}
