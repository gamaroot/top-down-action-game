
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
    private SpriteRenderer sprite;
    private Vector2 direction;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
    }
    // Start is called before the first frame update
    void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");
        rotateAction = playerInput.actions.FindAction("Rotate");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        GamePadLook();
        //MouseLook();
    }

    void MovePlayer()
    {
        direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * moveSpeed * Time.deltaTime;
    }

    private void GamePadLook()
    {

        float rotate = rotateAction.ReadValue<float>();

        if (rotate < 0 && sprite.flipX)
        {
            sprite.flipX = false;
        }
        else if (rotate > 0 && !sprite.flipX)
        {
            sprite.flipX = true;
        }
    }


    //
    // Currently Disabled until finished implementing funtionality
    //

    /*
    private void MouseLook()
    {

        float rotate = rotateAction.ReadValue<float>();
        // Calculate the distance between the camera and the player
        float distanceFromCamera = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        // Get the mouse position in world space, using the distance from the camera
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera));

        // Flip the sprite based on the mouse position relative to the player

        if ((mousePosition.x < transform.position.x && !sprite.flipX) || (rotate < 0 && sprite.flipX))
        {
            sprite.flipX = true;
        }
        if (mousePosition.x > transform.position.x && sprite.flipX || (rotate > 0 && !sprite.flipX))
        {
            sprite.flipX = false;
        }
    }
    */
}
    