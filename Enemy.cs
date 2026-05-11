using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENEMYT : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private int _enemyCount = 5;
    [SerializeField]
    private Transform _spawnTopLeft, _spawnTopRight, _spawnBottomLeft, _spawnBottomRight;
    
    void Start()
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            SpawnEnemy();
        }
    }
    
    private void SpawnEnemy()
    {
        Vector3 spawnPosition = SelectRandomPosition();
        GameObject enemyObject = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
        
        
        if (enemyObject.GetComponent<Rigidbody2D>() == null)
            enemyObject.AddComponent<Rigidbody2D>();
        
        if (enemyObject.GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = enemyObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
        }
        
        EnemyController controller = enemyObject.GetComponent<EnemyController>();
        if (controller == null)
            controller = enemyObject.AddComponent<EnemyController>();
        
        controller.OnDie += SpawnEnemy;
    }
    
    private Vector3 SelectRandomPosition()
    {
        Transform selectedTransform = null;
        int randomValue = Random.Range(0, 4);
        SpawnPointType spawnType = (SpawnPointType)randomValue;
        
        switch (spawnType)
        {
            case SpawnPointType.TopLeft:
                selectedTransform = _spawnTopLeft;
                break;
            case SpawnPointType.TopRight:
                selectedTransform = _spawnTopRight;
                break;
            case SpawnPointType.BottomRight:
                selectedTransform = _spawnBottomRight;
                break;
            case SpawnPointType.BottomLeft:
                selectedTransform = _spawnBottomLeft;
                break;
            default:
                selectedTransform = _spawnTopLeft;
                break;
        }
        
        return selectedTransform.position + (Vector3)Random.insideUnitCircle;
    }
    
    public enum SpawnPointType
    {
        TopLeft = 0,
        TopRight = 1,
        BottomLeft = 2,
        BottomRight = 3 
    }
}


public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;
    private Rigidbody2D _rb;
    private Transform _playerTransform;
    public event System.Action OnDie;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        
        // Find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            _playerTransform = player.transform;
    }
    
    void Update()
    {
        Move();
    }
    
    private void Move()
    {
        if (_playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _playerTransform = player.transform;
            else
                return;
        }
        
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        _rb.velocity = direction * _speed;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name.ToLower().Contains("weapon"))
        {
            Die();
        }
        
    }
    
    private void Die()
    {
        if (OnDie != null)
            OnDie();
        Destroy(gameObject);
    }
}
