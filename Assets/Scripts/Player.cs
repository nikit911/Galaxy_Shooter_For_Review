using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    private bool _isDead;
    public bool IsDead
    {
        get{
            return _isDead;
        }
    }
    [SerializeField]
    Vector3 startingPosition;
    [SerializeField]
    private float _speedMultiplier = 5f;
    private float _powerUpMultiplier = 2;
    [SerializeField]
    float _distance;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    bool _isTripleShotActive;
    [SerializeField]
    bool _isSpeedPowerUpActive;
    [SerializeField]
    bool _isShieldActive;
    [SerializeField]
    private GameObject _shieldVisualiser;
    [SerializeField]
    private GameObject[] _damageVisualiser;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    PostProcessVolume _volume;
    ChromaticAberration _ca;
    [SerializeField]
    AudioSource _laserShot;
    [SerializeField]
    AudioSource _deathExplosion;

    void Start()
    {
        startingPosition = new Vector3(0,0,0);
        transform.position  = startingPosition;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn manager is NULL");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is null");
        }
        _ca = ScriptableObject.CreateInstance<ChromaticAberration>();
        _ca.enabled.Override(false);
        _ca.intensity.Override(0.7f);
        _volume = PostProcessManager.instance.QuickVolume(_volume.gameObject.layer, 100f, _ca);
        
    }
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
           FireLaser();
        }
    }
    void CalculateMovement()
    {
        float xTranslation = Input.GetAxis("Horizontal");
        float yTranslation = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(xTranslation, yTranslation, 0);

        transform.Translate(direction* _speedMultiplier*Time.deltaTime);

        _distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
 
        if (transform.position.x >= 11.2f)
        {
            transform.position = new Vector3(-11.2f, transform.position.y, 0);
            Debug.Log("Wrapped from Right to left");
        }
        else if (transform.position.x <= -11.2f)
        {
            transform.position = new Vector3(11.2f, transform.position.y, 0);
            Debug.Log("Wrapped from left to Right");
        }

    }
    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive)
        {
            
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            
        }
        else if (_isTripleShotActive == false)
        {
            float laserOffset = _laserPrefab.GetComponent<Laser>().offset;
            Vector3 initialLaserPos = new Vector3(transform.position.x, transform.position.y+laserOffset, transform.position.z);
            Instantiate(_laserPrefab, initialLaserPos, Quaternion.identity);
        }
        _laserShot.Play();
        
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualiser.SetActive(false);
            Debug.Log("Shield Destroyed");
            return;
        }
        _lives--;

        if (_lives == 2)
        {
            _damageVisualiser[Random.Range(0, _damageVisualiser.Length)].SetActive(true);
        }
        else if(_lives == 1){
            foreach (var damageItem in _damageVisualiser)
            {
                if(damageItem.activeSelf == false)
                damageItem.SetActive(true);
            }
        }
            _uiManager.UpdateLives(_lives);
        if(_lives < 1)
        {
            _isDead = true;
            _spawnManager.OnPlayerDeath();
            _uiManager.GameOverDisplay();
            _deathExplosion.Play();
            
            Destroy(this.gameObject);
        }
    }
    public void CollectTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void CollectSpeedPowerup()
    {
        _isSpeedPowerUpActive = true;
        _ca.enabled.Override(true);
        _speedMultiplier *= _powerUpMultiplier;
        StartCoroutine(SpeedBoostPowerDown());
    }
    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _ca.enabled.Override(false);
        _isSpeedPowerUpActive = false;
        _speedMultiplier /= _powerUpMultiplier;
        
    }
    public void CollectShieldPowerup()
    {
        _isShieldActive = true;
        _shieldVisualiser.SetActive(true);
    }

    public void UpdateScore(int points){
        _score += points;
        _uiManager.UpdateScoreText(_score);
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy_Laser")
        {
            Damage();
            Destroy(other.gameObject);
        }
    }
}
