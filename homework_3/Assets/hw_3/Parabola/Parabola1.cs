using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        float old_z = this.transform.position.z;
        float dz = Time.deltaTime*speed;
        float dy = -dz*(old_z-20)/5;
        this.transform.position += new Vector3(0,dy,dz);
        
    }
}
