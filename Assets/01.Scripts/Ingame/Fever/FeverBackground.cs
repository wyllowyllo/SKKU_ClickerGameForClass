using DG.Tweening;
using UnityEngine;

public class FeverBackground : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _normalBackground;
    [SerializeField] private SpriteRenderer _feverBackground;
    [SerializeField] private float _fadeDuration = 0.5f;

    private void Start()
    {
        _normalBackground.color = Color.white;
        _feverBackground.color = new Color(1f, 1f, 1f, 0f);

        FeverManager.OnFeverModeChanged += Refresh;
    }

    private void OnDestroy()
    {
        FeverManager.OnFeverModeChanged -= Refresh;
    }

    private void Refresh()
    {
        bool isFever = FeverManager.Instance.IsFeverMode;

        _normalBackground.DOKill();
        _feverBackground.DOKill();

        _normalBackground.DOFade(isFever ? 0f : 1f, _fadeDuration);
        _feverBackground.DOFade(isFever ? 1f : 0f, _fadeDuration);
    }
}
