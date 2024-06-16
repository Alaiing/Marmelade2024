using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerActions foxyWhiteTrack = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private Transform grabTransform;

    private void Awake()
    {  
        foxyWhiteTrack = new PlayerActions();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        foxyWhiteTrack.Enable();
        foxyWhiteTrack.Game.Move.performed += OnMovementPerformed;
        foxyWhiteTrack.Game.Move.canceled += OnMovementCanceled;
        foxyWhiteTrack.Game.Interact.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        foxyWhiteTrack.Disable();
        foxyWhiteTrack.Game.Move.performed -= OnMovementPerformed;
        foxyWhiteTrack.Game.Move.canceled -= OnMovementCanceled;
        foxyWhiteTrack.Game.Interact.performed -= OnInteractPerformed;
    }

    #region Movement
    private void FixedUpdate()
    {
        rb.velocity = moveSpeed * moveVector;
        if (rb.velocity != Vector2.zero)
        {
            transform.rotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.down, rb.velocity, new Vector3(0, 0, 1)), new Vector3(0, 0, 1));
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();   
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
    #endregion

    #region Interaction
    [ShowInInspector]
    [ReadOnly]
    private List<Absorbable> _closeAbsorbables = new();
    private Absorbable _grabbedAbsorbable;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Absorbable absorbable = collision.GetComponentInParent<Absorbable>();
        if (absorbable != null && !_closeAbsorbables.Contains(absorbable))
        {
            _closeAbsorbables.Add(absorbable);
        }
        SortAbsorbables();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Absorbable absorbable = collision.GetComponentInParent<Absorbable>();
        if (absorbable != null && _closeAbsorbables.Contains(absorbable))
        {
            _closeAbsorbables.Remove(absorbable);
        }
        SortAbsorbables();
    }

    private void SortAbsorbables()
    {
        _closeAbsorbables.Sort(
            (a1, a2) 
            => Vector2.Distance(a1.transform.position, transform.position).CompareTo(Vector2.Distance(a2.transform.position, transform.position)));
        for (int i = 0; i < _closeAbsorbables.Count; i++)
        {
            _closeAbsorbables[i].Highlight(false);
        }
        if (_closeAbsorbables.Count > 0 && _grabbedAbsorbable == null)
        {
            _closeAbsorbables[0].Highlight(true);
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (_grabbedAbsorbable != null)
        {
            ThrowAbsorbable();
        }
        else
        {
            if (_closeAbsorbables.Count == 0)
                return;
            GrabAbsorbable(_closeAbsorbables[0]);
        }
    }

    private void GrabAbsorbable(Absorbable absorbable)
    {
        _grabbedAbsorbable = absorbable;
        _grabbedAbsorbable.Highlight(false);
        _grabbedAbsorbable.enabled = false;
        _grabbedAbsorbable.Body.isKinematic = true;
        _grabbedAbsorbable.Body.velocity = Vector2.zero;
        _grabbedAbsorbable.Body.angularVelocity = 0f;
        _grabbedAbsorbable.GetComponent<FaceCamera>().enabled = false;
        _grabbedAbsorbable.transform.SetParent(grabTransform, worldPositionStays:true);
        _grabbedAbsorbable.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.AngleAxis(180, new Vector3(0,0,1)));
    }

    private void ThrowAbsorbable()
    {
        _grabbedAbsorbable.transform.SetParent(null, worldPositionStays: true);
        _grabbedAbsorbable.Body.isKinematic = false;
        _grabbedAbsorbable.Body.AddForce(-transform.up * 10, ForceMode2D.Impulse);
        _grabbedAbsorbable.enabled = true;
        _grabbedAbsorbable.GetComponent<FaceCamera>().enabled = true;
        _grabbedAbsorbable = null;
    }
    #endregion
}