using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float yDirection = -_speed*Time.deltaTime;
        transform.Translate(0,yDirection,0);
        if(transform.position.y <= -5f)
        {
            Destroy(this.gameObject);
        }
    }
}
