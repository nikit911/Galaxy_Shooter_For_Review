using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private int _powerUpId;
    private AudioSource _powerupSource;

    void Update()
    {
        transform.Translate(0, -_speed*Time.deltaTime, 0);
        if (transform.position.y <= -5.35f)
        {
            Destroy(this.gameObject);
        }
        _powerupSource = GameObject.Find("Powerup_Sound").GetComponent<AudioSource>();
        if (_powerupSource == null)
        {
            Debug.LogError("powerup source is null on powerup");
        }

    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {   
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch(_powerUpId)
                {
                    case 0: 
                        player.CollectTripleShot();
                        break;
                    case 1: 
                        player.CollectSpeedPowerup();
                        break;
                    case 2: 
                        Debug.Log("Shield collected");
                        player.CollectShieldPowerup();
                        break;
                    default: 
                        Debug.Log("no other id");
                        break;
                }                   
            }
            _powerupSource.Play();
            Destroy(this.gameObject);
        }
    }
}
