using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Star : MonoBehaviour
{
    [SerializeField]
    private float _startSize;
    [SerializeField]
    private float _maxSize;

    private float _currentSize;

    private int _absorbedAmount;
    [ShowInInspector]
    [ReadOnly]
    private int _currentStarDataIndex;

    private float _pulsationTimer;

    [SerializeField]
    private Transform _renderTransform;
    [SerializeField]
    private DOTweenAnimation _tweenAnimation;

    private Transform _transform;

    [SerializeField]
    private float _moveDuration = 0.2f;
    [SerializeField]
    private float _moveDelay = 0.1f;

    [ShowInInspector]
    [ReadOnly]
    private int[] _absorbedTags;

    private GameData.StarData CurrentData => GameData.DATA.StarDatas[_currentStarDataIndex];


    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        _currentStarDataIndex = 0;
        _pulsationTimer = 0;
        _absorbedAmount = 0;
        _absorbedTags = new int[GameData.DATA.objectTags.Length];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Absorbable absorbable = collision.collider.GetComponentInParent<Absorbable>();
        if (absorbable != null)
        {
            AbsorbObject(absorbable);
            Destroy(absorbable.gameObject);
        }
    }

    private void Update()
    {
        _pulsationTimer += Time.deltaTime;
        if (_pulsationTimer >= CurrentData.PulsationPeriod)
        {
            Pulsate();
            _pulsationTimer = 0;
        }
    }

    [Button]
    private void Pulsate()
    {
        Bump();
        foreach(Absorbable absorbable in Absorbable.Absorbables)
        {
            Vector3 attractionDistance = (_transform.position - absorbable.transform.position).normalized * Mathf.Lerp(GameData.DATA.MinAttractionDistance, GameData.DATA.MaxAttractionDistance, absorbable.AttractionAmount) * CurrentData.Gravity;
            absorbable.MoveBy(attractionDistance, _moveDuration, _moveDelay);
        }
    }

    private void Bump()
    {
        _tweenAnimation.DORewind();
        _tweenAnimation.DOPlay();
    }

    public void AbsorbObject(Absorbable absorbable)
    {
        _absorbedAmount += absorbable.AbsorptionAmount;
        _absorbedTags[absorbable.ObjectTag]++;
        Bump();
        if (_absorbedAmount > CurrentData.Threshold)
        {
            _currentStarDataIndex++;
            // TODO : check max possible
        }
    }
}
