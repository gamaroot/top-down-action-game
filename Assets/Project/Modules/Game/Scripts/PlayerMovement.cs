
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction rotateAction;
    private SpriteRenderer spriteRenderer;
    private Vector2 direction;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        rotateAction = playerInput.actions.FindAction("Rotate");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        //RotatePlayer();
        //SpriteFlip();
    }

    void MovePlayer()
    {
        direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * moveSpeed * Time.deltaTime;
    }

    //void SpriteFlip()
    //{
    //    if (direction.x < 0)
    //    {
    //        spriteRenderer.flipX = false;
    //    }
    //    if (direction.x > 0)
    //    {
    //        spriteRenderer.flipX = true;
    //    }
    //}

   
    //Not Rotating the Player right now

    //void RotatePlayer()
    //{
    //    float rotate = rotateAction.ReadValue<float>();

    //    if (rotate == -1)
    //    {
    //        transform.Rotate(0, -1, 0);
    //    }

    //    if (rotate == 1)
    //    {
    //        transform.Rotate(0, 1, 0);
    //    }
    //}
}
    