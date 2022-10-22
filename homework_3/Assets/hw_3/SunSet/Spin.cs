using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    private Vector3 axis;
    private float cycle;
    public float angle,cycle_pre;// cycle_pre—自转周期(天)
    // Start is called before the first frame update
    void Start()
    {
        Transform origin_transform = this.gameObject.transform;

        GameObject axis_entity = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        
        axis_entity.transform.parent = origin_transform;
        axis_entity.transform.localPosition = Vector3.zero;
        axis_entity.transform.localScale = 
            new Vector3(0.05f,1f,0.05f);
          

        axis = new Vector3(Mathf.Tan(angle/180*Mathf.PI),1.0f,0.0f);
        this.gameObject.transform.up = axis;
        // origin_transform.rotation = Quaternion.FromToRotation(origin_transform.up,axis);
        cycle = cycle_pre/200;
           
    }

    // Update is called once per frame
    void Update()
    {
        // Quaternion q = Quaternion.AngleAxis(1/cycle*Time.deltaTime,axis);
        
        // this.gameObject.transform.rotation *= q;
        
        this.transform.Rotate(axis,1/cycle*Time.deltaTime,Space.World);
    }
}
