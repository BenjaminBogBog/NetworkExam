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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PipeTrigger"))
        {
            GameManager.Instance.SpawnNewPipe();
        }
    }
}
