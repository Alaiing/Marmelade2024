using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerActions foxyWhiteTrack = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;
    private float moveSpeed = 10f;

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
    }

    private void OnDisable()
    {
        foxyWhiteTrack.Disable();
        foxyWhiteTrack.Game.Move.performed -= OnMovementPerformed;
        foxyWhiteTrack.Game.Move.canceled -= OnMovementCanceled;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveSpeed * moveVector;
        if(rb.velocity != Vector2.zero) transform.up = -rb.velocity.normalized;
    }



    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();   
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
}