using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed=1;
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    AudioSource _explosionSound;
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
      /*   _angleOverTime += Time.deltaTime*_rotationSpeed;
        transform.rotation = Quaternion.Euler(0,0,_angleOverTime); */
        transform.Rotate(Vector3.forward*_rotationSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosion, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            _explosionSound.Play();
            Destroy(this.gameObject, 0.2f);
        }
    }
}
