using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    public static event Action OnDataChanged ;

    [SerializeField] private UpgradeSpecTableSO _specTable;
    
    private Dictionary<EUpgradeType, Upgrade> _upgrades = new ();
    
    private void Awake()
    {
        Instance = this;

        // 스펙 데이터에 따라 도메인 생성
        foreach (var specData in _specTable.Datas)
        {
            if (_upgrades.ContainsKey(specData.Type))
            {
                throw new Exception($"There is already an upgrade with type {specData.Type}");
            }
            
            Debug.Log(specData.Name);
            _upgrades.Add(specData.Type, new Upgrade(specData));
        }
        
        OnDataChanged?.Invoke();
    }
    
    public Upgrade Get(EUpgradeType type) => _upgrades[type] ?? null;
    public List<Upgrade> GetAll() => _upgrades.Values.ToList();
    
    public bool CanLevelUp(EUpgradeType type)
    {
        if (!_upgrades.TryGetValue(type, out Upgrade upgrade))
        {
            return false;
        }

        if (!upgrade.CanLevelUp())
        {
            return false;
        }

        return CurrencyManager.Instance.CanAfford(ECurrencyType.Gold, upgrade.Cost);
    }

    public bool TryLevelUp(EUpgradeType type)
    {
        if (!_upgrades.TryGetValue(type, out Upgrade upgrade))
        {
            Debug.Log(1);

            return false;
        }

        if (!CurrencyManager.Instance.TrySpend(ECurrencyType.Gold, upgrade.Cost))
        {
            Debug.Log(2);

            return false;
        }

        if (!upgrade.TryLevelUp())
        {
            Debug.Log(3);
            return false;
        }
        
        OnDataChanged?.Invoke();
        
        return true;
    }
}