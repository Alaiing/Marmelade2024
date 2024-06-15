using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Absorbable : MonoBehaviour
{
    public static List<Absorbable> Absorbables = new();

    [SerializeField]
    [OnValueChanged(nameof(OnDataChanged))]
    private ObjectData _data;
    public float AttractionAmount => _data.AttractionRate;
    public int ObjectTag => _data.tag;
    [SerializeField]
    private AnimationCurve _moveCurve;

    private SpriteRenderer _spriteRenderer;

    public int AbsorptionAmount => _data.AborbAmount;

    private Rigidbody2D _body;
    private Camera _camera;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_data != null)
            _spriteRenderer.sprite = _data.sprite;
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        Absorbables.Add(this);
    }

    private void OnDisable()
    {
        Absorbables.Remove(this);
    }

    private void Update()
    {
        Vector2 relativePosition = _camera.transform.position - transform.position;
        _spriteRenderer.transform.up = -relativePosition.normalized;
        _spriteRenderer.transform.localScale = new Vector3(1f,Mathf.Lerp(1,1.5f, relativePosition.magnitude / 50),1f);
    }

    public void SetData(ObjectData data)
    {
        _data = data;
        UpdateData();
    }

    private void UpdateData()
    {
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
    private void OnDataChanged()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        UpdateData();
    }
#endif
}
