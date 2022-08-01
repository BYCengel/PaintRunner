using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private TestPlayer player;
    private Rigidbody2D rigidBody2D;

    private float smoothSpeed = 0.125f;
    private float moveSpeed = 6f;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        player = playerTransform.GetComponent<TestPlayer>();
    }

    private void FixedUpdate()
    {
        if (!player.GetDeath())
        {
            Follow();

            if (player.GetStarted())
            {
                Move();
            }
        }
    }

    private void Follow()
    {
        float desiredPosition = playerTransform.position.y;
        float smoothedPosition = Mathf.Lerp(transform.position.y, desiredPosition, smoothSpeed);
        Vector3 toTarget = new Vector3(transform.position.x, smoothedPosition, transform.position.z);
        transform.position = toTarget;
    }
    
    private void Move()
    {
        rigidBody2D.velocity = new Vector2(+moveSpeed, rigidBody2D.velocity.y);
    }

    public void SetMoveSpeed(float newSpeed){
        moveSpeed = newSpeed;
    }
    public float GetCurrentSpeed(){
        return moveSpeed;
    }
}
