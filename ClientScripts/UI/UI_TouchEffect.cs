using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TouchEffect : MonoBehaviour
{
    private SpriteRenderer sprite;

    private Vector2 dir;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private float sizeSpeed;

    [SerializeField] private Color[] colors;
    [SerializeField] private float colorSpeed;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        float _size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector2(_size, _size);
        sprite.color = colors[Random.Range(0, colors.Length)];
    }
    void Update()
    {
        transform.Translate(dir * moveSpeed);
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime * sizeSpeed);

        Color _color = sprite.color;
        _color.a = Mathf.Lerp(sprite.color.a, 0, Time.deltaTime * colorSpeed);
        sprite.color = _color;

        if (sprite.color.a <= 0.01f)
            Destroy(gameObject);
    }
}
