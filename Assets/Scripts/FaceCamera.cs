using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnDisable()
    {
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        Vector2 relativePosition = _camera.transform.position - transform.position;
        transform.up = -relativePosition.normalized;
        transform.localScale = new Vector3(1f, Mathf.Lerp(1, 1.5f, relativePosition.magnitude / 5), 1f);
    }
}
