using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //speed variable of 8
    [SerializeField]
    private float _speed = 8.0f;
    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        float yDirection = _speed*Time.deltaTime;
        transform.Translate(0, yDirection, 0);
        if(transform.position.y >= 7.3f)
        {
            if(transform.parent != null)
            {

                Destroy(transform.parent.gameObject);
                
            }
            Destroy(this.gameObject);
        }
    }
}
