using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        float z = Time.time*2;
        float y = -(z-20)*(z-20)/10 + 40;
        this.transform.position = new Vector3(0,y,z);
    }
}
