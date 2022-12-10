using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float jumpForce;
    private void Start()
    {
        _rb ??= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }

        if (transform.position.y <= -4.4)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PipeTrigger"))
        {
            GameManager.Instance.SpawnNewPipe();
            GameManager.Instance.score++;
        }
        
        if (col.CompareTag("PipeBlock"))
        {
            //GameOver
            GameManager.Instance.TriggerGameOver();
        }
    }
}
