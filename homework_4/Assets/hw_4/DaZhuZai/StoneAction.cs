using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_4
{
    public class StoneAction : MonoBehaviour
    {
        Vector3 destination;
        Vector3 speed;
        // Start is called before the first frame update
        void Start()
        {
            
        }
        
        // Update is called once per frame
        void Update()
        {
            this.gameObject.transform.position += speed*Time.deltaTime*2;
            if(Vector3.Distance(this.gameObject.transform.position,destination)<0.1f)
                Destroy(this.gameObject);
        }
        public void set_destination(Vector3 des)
        {
            destination = des;
        }
        public void set_speed(Vector3 s)
        {
            speed = s;
        }
    }
}

