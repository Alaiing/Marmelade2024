using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Absorbable : MonoBehaviour
{
    public static List<Absorbable> Absorbables = new();

    [SerializeField]
    private ObjectData _data;
    public float AttractionAmount => _data.AttractionRate;
    [SerializeField]
    private AnimationCurve _moveCurve;

    private SpriteRenderer _spriteRenderer;

    public int AbsorptionAmount => _data.AborbAmount;

    private Rigidbody2D _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_data != null)
            _spriteRenderer.sprite = _data.sprite;
    }

    private void OnEnable()
    {
        Absorbables.Add(this);
    }

    private void OnDisable()
    {
        Absorbables.Remove(this);
    }

    public void SetData(ObjectData data)
    {
        _data = data;
        _spriteRenderer.sprite = _data.sprite;
    }
    public void MoveBy(Vector2 moveVector, float duration)
    {
        StartCoroutine(MoveTo(moveVector, duration));
    }

    IEnumerator MoveTo(Vector2 moveVector, float duration)
    {
        float timer = 0;
        Vector2 startPosition = transform.position;
        Vector2 endPosition = (Vector2)transform.position + moveVector;
        while (timer < duration)
        {
            _body.MovePosition(Vector2.Lerp(startPosition, endPosition, _moveCurve.Evaluate(timer / duration)));
            yield return null;
            timer += Time.deltaTime;
        }

        _body.MovePosition(endPosition);
    }
}
