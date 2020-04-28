using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool _isDead;
    [SerializeField]
    private float _movementSpeed = 2.0f;
    private Vector3 _spawnPos;
    private Vector3 _laserPos;
    private Player _player;
    private Animator _deathAnim;
    private float _canFire = -1f;
    //private float _fireRate = 0.5f;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private float _onDestroyedSpeed;
    AudioSource _explosionSound;
    [SerializeField]
    AudioClip _explosionClip;
    void Start()
    {
        _spawnPos = SpawnPosition();
        transform.position = _spawnPos;
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player NULL");
        }
        _deathAnim = GetComponent<Animator>();
        if (_deathAnim == null)
        {
            Debug.LogError("Enemy Death animator null");
        }
        _explosionSound = GetComponent<AudioSource>();
        if(_explosionSound == null)
        {
            Debug.LogError("the AudioSource for the enemy is NULL");
        }
        else if(_explosionSound != null)
        {
            _explosionSound.clip = _explosionClip;
        }
    }

    void Update()
    {
        transform.Translate(0, -_movementSpeed*Time.deltaTime, 0);  
        if (transform.position.y <= -5.43f)
        {
            transform.position = SpawnPosition();
        }
        if (Time.time > _canFire && _player.IsDead == false && _isDead == false)
        {
              fireEnemyLaser();
        }
      
     
    }
    private Vector3 SpawnPosition(){
        return new Vector3(Random.Range(-8f, 8f), Random.Range(5f, 6.03f), 0);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            
            _deathAnim.SetTrigger("OnEnemyDeath");
            _movementSpeed = _onDestroyedSpeed;
            _isDead = true;
            _explosionSound.Play();
            Destroy(this.gameObject, 2.3f);
            Debug.Log("player damaged");
        }
        else if (other.tag == "Laser")
        {
            
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.UpdateScore(10);
            }
            
            _deathAnim.SetTrigger("OnEnemyDeath");
            _isDead = true;
            _explosionSound.Play();
            _movementSpeed = _onDestroyedSpeed;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.3f);
                        
        }

    }
    public void fireEnemyLaser(){
        _canFire = Time.time + Random.Range(1f,3f);
        _laserPos = new Vector3(transform.position.x, transform.position.y-0.8f, transform.position.z);
        Instantiate(_enemyLaserPrefab, _laserPos, Quaternion.identity);
        
    }
}
