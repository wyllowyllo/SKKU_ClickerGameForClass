using UnityEngine;

// '업그레이드'라는 게임 콘텐츼의 도메인 클래스다.
// 도메인이란 핵심 데이터와 규칙을 말한다.
// 가장 먼저 만들고, 가장 나중에 바뀐다. (게임의 본질이기 때문에)
public class Upgrade 
{
    // 1. 기획 테이블의 데이터를 가져온다.
    public readonly EUpgradeType Type;
    public readonly int MaxLevel;
    public readonly double BaseCost;
    public readonly double BaseDamage;
    public readonly double CostMultiplier;
    public readonly double DamageMultiplier;
    public readonly string Name;
    public readonly string Description;

    // 2. 핵심 규칙(유효성)을 작성한다.
    public Upgrade(EUpgradeType type, int maxLevel, double baseCost, double baseDamage, double costMultiplier, double damageMultiplier, string name, string description)
    {
        if (maxLevel < 0) throw new System.ArgumentException($"최대 레벨은 0보다 커야 합니다: {maxLevel}");
        if (baseCost <= 0) throw new System.ArgumentException($"기본 비용은 0보다 커야 합니다: {baseCost}");
        if (baseDamage <= 0) throw new System.ArgumentException($"기본 대미지는 0보다 커야 합니다: {baseDamage}");
        if (costMultiplier <= 0) throw new System.ArgumentException($"비용 증가량은 0보다 커야 합니다: {costMultiplier}");
        if (damageMultiplier <= 0) throw new System.ArgumentException($"대미지 증가량은 0보다 커야 합니다: {damageMultiplier}");
        if (string.IsNullOrEmpty(name)) throw new System.ArgumentException("이름은 비어있을 수 없습니다");
        if (string.IsNullOrEmpty(description)) throw new System.ArgumentException("설명은 비어있을 수 없습니다");
        

        Type = type;
        MaxLevel = maxLevel;
        BaseCost = baseCost;
        BaseDamage = baseDamage;
        CostMultiplier = costMultiplier;
        DamageMultiplier = damageMultiplier;
        Name = name;
        Description = description;
    }
    
}
