using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    private SpriteRenderer _spriteRenderer;

    private Camera _camera;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnDisable()
    {
        _spriteRenderer.transform.localRotation = Quaternion.identity;
        _spriteRenderer.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        Vector2 relativePosition = _camera.transform.position - transform.position;
        _spriteRenderer.transform.up = -relativePosition.normalized;
        _spriteRenderer.transform.localScale = new Vector3(1f, Mathf.Lerp(1, 1.5f, relativePosition.magnitude / 5), 1f);
    }
}
