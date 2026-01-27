using UnityEngine;

public class ShiningAbility : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Material material;
    private static readonly int ShineLocation = Shader.PropertyToID("_ShineLocation");

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        material = spriteRenderer.material;
    }

    void Update()
    {
        float location = Mathf.Repeat(Time.time * speed, 1f);
        material.SetFloat(ShineLocation, location);
    }
}
