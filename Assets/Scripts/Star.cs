using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Star : MonoBehaviour
{
    public static event Action OnPlayerEaten;
    public static event Action<int> OnStarExploded;

    [SerializeField]
    private float _startSize;
    [SerializeField]
    private float _maxSize;

    private float _currentSize;

    private int _absorbedAmount;
    [ShowInInspector]
    [ReadOnly]
    private int _currentPhase;

    private float _pulsationTimer;

    [SerializeField]
    private Transform _renderTransform;
    [SerializeField]
    private DOTweenAnimation _tweenAnimation;

    private Transform _transform;
    private Animator _animator;

    [SerializeField]
    private float _moveDuration = 0.2f;
    [SerializeField]
    private float _moveDelay = 0.1f;

    [ShowInInspector]
    [ReadOnly]
    static public int[] absorbedTags;

    [SerializeField]
    private Animator[] _phases;

    private GameData.StarData CurrentData => GameData.DATA.StarDatas[_currentPhase];

    [SerializeField]
    private AK.Wwise.Event _absorbSound;
    [SerializeField]
    private AK.Wwise.Event _ZBOUI;
    [SerializeField]
    private AK.Wwise.Event _phaseChangeSound;
    [SerializeField]
    private AK.Wwise.Event _explodeSound;
    [SerializeField]
    private AK.Wwise.Event _ZBOUII;

    private void Awake()
    {
        _transform = transform;
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _currentPhase = 0;
        UpdatePhase();
        _pulsationTimer = 0;
        _absorbedAmount = 0;
        absorbedTags = new int[GameData.DATA.objectTags.Length];

        OnStarExploded += GoEnding;
    }

    private void GoEnding(int tag) {
        if(tag == 1) SceneManager.LoadScene("End1", LoadSceneMode.Single);
        if(tag == 2) SceneManager.LoadScene("End2", LoadSceneMode.Single);
        if(tag == 3) SceneManager.LoadScene("End3", LoadSceneMode.Single);
        if(tag == 4) SceneManager.LoadScene("End4", LoadSceneMode.Single);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Absorbable absorbable = collision.collider.GetComponentInParent<Absorbable>();
        if (absorbable != null)
        {
            if (absorbable.TryGetComponent(out PlayerMovement _))
            {
                _ZBOUI.Post(gameObject);
                OnPlayerEaten?.Invoke();
            }
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

    private void UpdatePhase()
    {
        for (int i = 0; i < _phases.Length; i++)
        {
            if (i == _currentPhase)
            {
                _animator = _phases[i];
                _phases[i].gameObject.SetActive(true);
            }
            else
            {
                _phases[i].gameObject.SetActive(false);
            }
        }
    }

    [Button]
    private void Pulsate()
    {
        Bump();
        foreach (Absorbable absorbable in Absorbable.Absorbables)
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

    private int GetMostEatenTag()
    {
        return Array.IndexOf(absorbedTags, absorbedTags.Max());
    }

    public void AbsorbObject(Absorbable absorbable)
    {
        _absorbedAmount += absorbable.AbsorptionAmount;
        absorbedTags[absorbable.ObjectTag]++;
        _animator.SetTrigger("Hit");
        _absorbSound.Post(absorbable.gameObject);
        if (_absorbedAmount > CurrentData.Threshold)
        {
            _currentPhase++;
            _phaseChangeSound.Post(gameObject);
            if (_currentPhase > GameData.DATA.StarDatas.Length)
            {
                _explodeSound.Post(gameObject);
                OnStarExploded?.Invoke(GetMostEatenTag());
                return;
            }
            _ZBOUII.Post(gameObject);
            UpdatePhase();
        }
    }
}
