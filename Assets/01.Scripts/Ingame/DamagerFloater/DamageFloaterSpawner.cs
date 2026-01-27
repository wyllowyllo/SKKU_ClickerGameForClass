using Lean.Pool;
using UnityEngine;

public class DamageFloaterSpawner : MonoBehaviour
{
    public static DamageFloaterSpawner Instance { get; private set; }

    [SerializeField] private LeanGameObjectPool _pool;
    [SerializeField] private Vector2 _randomOffset;

    private void Awake()
    {
        Instance = this;

        _pool = GetComponent<LeanGameObjectPool>();
    }

    public void ShowDamage(ClickInfo clickInfo)
    {
        // 1. 풀로부터 DamageFloater를 가져와서
        Vector2 offset = new Vector2(
            Random.Range(-_randomOffset.x, _randomOffset.x),
            Random.Range(-_randomOffset.y, _randomOffset.y)
        );
        Vector2 spawnPosition = clickInfo.Position + offset;

        GameObject floaterObject = _pool.Spawn(spawnPosition, Quaternion.identity);
        DamageFloater floater = floaterObject.GetComponent<DamageFloater>();

        // 2. 클릭한 위치에 생성한다.
        floater.Show(clickInfo);
    }
}
