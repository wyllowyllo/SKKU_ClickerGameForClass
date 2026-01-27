using TMPro;
using UnityEngine;

public class HUD_Currency : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldText;


    private void Start()
    {
        Refresh();

        GameManager.OnDataChanged += Refresh;
    }

    private void Refresh()
    {
        _goldText.text = GameManager.Instance.Gold.ToFormattedString();
    }
}