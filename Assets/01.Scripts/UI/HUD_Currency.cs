using DG.Tweening;
using TMPro;
using UnityEngine;

public class HUD_Currency : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private float _punchScale = 0.2f;
    [SerializeField] private float _punchDuration = 0.3f;

    private void Start()
    {
        Refresh();

        CurrencyManager.OnDataChanged += Refresh;
    }

    private void Refresh()
    {
        _goldText.text = CurrencyManager.Instance.Gold.ToFormattedString();

        _goldText.transform.DOKill();
        _goldText.transform.localScale = Vector3.one;
        _goldText.transform.DOPunchScale(Vector3.one * _punchScale, _punchDuration, 1, 0.5f);
    }
}
