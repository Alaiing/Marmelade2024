using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Absorbable : MonoBehaviour
{
    public static List<Absorbable> Absorbables = new();

    [SerializeField]
    [OnValueChanged("OnDataChanged")]
    private ObjectData _data;
    public float AttractionAmount => _data.AttractionRate;
    public int ObjectTag => _data.tag;
    [SerializeField]
    private AnimationCurve _moveCurve;

    private SpriteRenderer _spriteRenderer;

    public int AbsorptionAmount => _data.AbsorbAmount;

    private Rigidbody2D _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        UpdateData();
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
        UpdateData();
    }

    private void UpdateData()
    {
        if (_data == null || _data.sprite == null)
            return;

        _spriteRenderer.sprite = _data.sprite;
    }

    public void MoveBy(Vector2 moveVector, float duration, float delay)
    {
        StartCoroutine(MoveTo(moveVector, duration, delay));
    }

    IEnumerator MoveTo(Vector2 moveVector, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
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

#if UNITY_EDITOR
    [Button("Update Collider")]
    private void OnDataChanged()
    {
        if (_data == null || _data.sprite == null)
            return;

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        PolygonCollider2D collider = GetComponentInChildren<PolygonCollider2D>();
        UpdateData();
        List<Vector2> points = new();
        _spriteRenderer.sprite.GetPhysicsShape(0, points);
        collider.points = points.ToArray();
        name = _data.name;
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(collider);
    }
#endif
}
