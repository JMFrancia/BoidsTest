using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockLeaderController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    
    private Vector3 _move;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move(GetPlayerInput());
    }
    
    private Vector3 GetPlayerInput()
    {
        Vector3 result = Vector3.zero;
        
        if(Input.GetKey(KeyCode.W))
        {
            result += new Vector3(0, 1f, 0f);
        }
        if(Input.GetKey(KeyCode.S))
        {
            result += new Vector3(0, -1f, 0f);;
        }
        if(Input.GetKey(KeyCode.A))
        {
            result += new Vector3(-1f, 0f, 0f);;
        }
        if(Input.GetKey(KeyCode.D))
        {
            result += new Vector3(1f, 0f, 0f);;
        }

        return result;
    }
    
    private void Move(Vector3 velocity)
    {
        _rb.velocity = velocity * _speed; //(_speed * Time.deltaTime);
        /*
        if (Mathf.Approximately(velocity.magnitude, 0f))
        {
            _rb.velocity = Vector3.zero;
        }
        else
        {
            _rb.AddForce(velocity * (_speed * Time.deltaTime), ForceMode.VelocityChange);
        }
        */
    }
}
