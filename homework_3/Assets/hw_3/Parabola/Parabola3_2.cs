using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola3_2 : MonoBehaviour
{
    private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float old_z = this.transform.position.z;
        float dz = Time.deltaTime*speed;
        float dy = -dz*(old_z-20)/5;
        this.transform.position += new Vector3(0,dy,0);
    }
}
