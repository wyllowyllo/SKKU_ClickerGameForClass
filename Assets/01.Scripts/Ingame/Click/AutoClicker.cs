using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    [SerializeField] private float _interval = 2f;
    [SerializeField] private DashAbility _dashAbility;

    private float _timer;

    private void Update()
    {
        if (_dashAbility.IsDashing)
            return;

        _timer += Time.deltaTime;

        if (_timer >= _interval)
        {
            _timer = 0f;

            GameObject[] clickables = GameObject.FindGameObjectsWithTag("Clickable");
            if (clickables.Length > 0)
            {
                GameObject target = clickables[Random.Range(0, clickables.Length)];
                Clickable clickable = target.GetComponent<Clickable>();

                double damage = GetAutoClickDamage();

                
                _dashAbility.Execute(target.transform, () =>
                {
                    ClickInfo clickInfo = new ClickInfo
                    {
                        Type = EClickType.Auto,
                        Damage = damage,
                        Position = transform.position,
                    };
                    clickable.OnClick(clickInfo);
                });
            }
        }
    }
    
    private double GetAutoClickDamage()
    {
        double flat = UpgradeManager.Instance.Get(EUpgradeType.AutoClickDamagePlusAdd).Damage;
        double percent = UpgradeManager.Instance.Get(EUpgradeType.AutoClickDamagePercentAdd).Damage;
        double percent2 = UpgradeManager.Instance.Get(EUpgradeType.AutoClick2DamagePercentAdd).Damage;

        
        double baseDamage = 1; // 기본 오토 대미지
        return (baseDamage + flat) * (1 + percent / 100.0) * (1 + percent2 / 100.0);
    }
}
