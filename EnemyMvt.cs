using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class enemyta7arok : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;
    private Rigidbody2D _rb2d;
    private Transform _playerTransform;
    public bool Stopped = false;
    [SerializeField]
    private GameObject _crabDead;

    public event Action OnDie = null;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _playerTransform = playerObject.transform;
        }
        else
        {
            Stopped = true;
            Debug.LogWarning("Player not found! Make sure your player has the 'Player' tag.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    private void Move()
    {
        if (Stopped || _playerTransform == null)
        {
            if (_rb2d != null)
                _rb2d.velocity = Vector2.zero;
            return;
        }
        
        Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;
        _rb2d.velocity = directionToPlayer * _speed;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check by tag only (make sure "weapon" tag exists in Unity)
        if (collision.CompareTag("Weapon"))
        {
            if (_crabDead != null)
                Instantiate(_crabDead, transform.position, Quaternion.identity);
            Destroy(gameObject);
            if (OnDie !=null)
            {
               OnDie(); 
            }
        }
    }
}
