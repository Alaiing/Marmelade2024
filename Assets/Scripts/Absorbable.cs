using DG.Tweening;
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

    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private SpriteRenderer _outlineRenderer;

    public int AbsorptionAmount => _data.AbsorbAmount;

    private Rigidbody2D _body;
    public Rigidbody2D Body => _body;
    [SerializeField]
    private DOTweenAnimation _highlightAnimation;

    private Collider2D _collider;
    public Collider2D Collider => _collider;

    private TrailRenderer _trailRenderer;
    private FaceCamera _faceCamera;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponentInChildren<Collider2D>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
        _faceCamera = GetComponentInChildren<FaceCamera>();
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
        _outlineRenderer.sprite = _data.sprite;
        _outlineRenderer.color = GameData.DATA.objectTags[_data.tag].color;
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

    public void Highlight(bool highlight)
    {
        if (highlight)
        {
            _highlightAnimation.DORewind();
            _highlightAnimation.DOPlay();
        }
        else
        {
            _highlightAnimation.DOPause();
            _highlightAnimation.DORewind();
        }
    }

    public void OnGrabbed()
    {
        Highlight(false);
        _trailRenderer.enabled = false;
        enabled = false;
        Body.isKinematic = true;
        Body.velocity = Vector2.zero;
        Body.angularVelocity = 0f;
        _faceCamera.enabled = false;
        Collider.gameObject.layer = LayerMask.NameToLayer("Grabbed");
    }

    public void OnReleased()
    {
        Body.isKinematic = false;
        enabled = true;
        _trailRenderer.enabled = true;
        _faceCamera.enabled = true;
        Collider.gameObject.layer = LayerMask.NameToLayer("Object");
    }

#if UNITY_EDITOR
    [Button("Update Collider")]
    private void OnDataChanged()
    {
        if (_data == null || _data.sprite == null)
            return;

        PolygonCollider2D collider = GetComponentInChildren<PolygonCollider2D>();
        OnSpriteChanged();
        List<Vector2> points = new();
        _spriteRenderer.sprite.GetPhysicsShape(0, points);
        collider.points = points.ToArray();
        name = _data.name;
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(collider);
    }

    [Button("Update sprite")]
    private void OnSpriteChanged()
    {
        UpdateData();
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(_spriteRenderer);
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(_outlineRenderer);
    }
#endif
}
