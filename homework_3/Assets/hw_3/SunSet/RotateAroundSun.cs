using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundSun : MonoBehaviour
{
    private Vector3 point,axis;
    private float cycle,a,alpha;
    public float angle,a_pre,cycle_pre;// angle—轨道倾角, a_pre—半长轴(天文单位), cycle_pre—公转周期(天)
    // Start is called before the first frame update
    void Start()
    {
        

        alpha = (angle/180)*Mathf.PI;
        a = 400*a_pre;
        cycle = cycle_pre/10000;
        this.transform.position = new Vector3(a,a*Mathf.Tan(alpha),0);
        point = new Vector3(0,0,0);
        axis = new Vector3(-Mathf.Tan(alpha),1,0);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion q = Quaternion.AngleAxis(1/cycle*Time.deltaTime,axis);
        // this.transform.RotateAround(point,axis,Time.deltaTime);
        this.transform.position = q*(this.transform.position);
    }
}
