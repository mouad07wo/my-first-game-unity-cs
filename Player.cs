using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class player : MonoBehaviour
{
    [SerializeField]
    private string _horizontalAxis = "Horizontal", _verticalAxis = "Vertical";
    [SerializeField]
    private Rigidbody2D _rb2d;
    private Vector2 _input;

    [SerializeField]
    private float _speed = 3f;
    public UnityEvent OnPlayeDie;
    private void FixedUpdate()
    {
        _rb2d.velocity = _input * _speed;

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw(_horizontalAxis);
        float verticalInput = Input.GetAxisRaw(_verticalAxis);
        _input = new Vector2(horizontalInput, verticalInput);
        _input.Normalize();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(OnPlayeDie != null)
          { 
           OnPlayeDie.Invoke();
        } 
        Destroy(gameObject);
    }

}
