using DG.Tweening;
using UnityEngine;

public class FeverBackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _normalPitch = 1f;
    [SerializeField] private float _feverPitch = 1.3f;
    [SerializeField] private float _transitionDuration = 0.3f;

    private void Start()
    {
        FeverManager.OnFeverModeChanged += Refresh;
    }

    private void OnDestroy()
    {
        FeverManager.OnFeverModeChanged -= Refresh;
    }

    private void Refresh()
    {
        bool isFever = FeverManager.Instance.IsFeverMode;
        float targetPitch = isFever ? _feverPitch : _normalPitch;

        DOTween.Kill(_audioSource);
        DOTween.To(() => _audioSource.pitch, x => _audioSource.pitch = x, targetPitch, _transitionDuration)
            .SetTarget(_audioSource);
    }
}
