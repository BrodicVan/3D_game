using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class FinalCollision : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag =="Player")
            {
                
                GameEventManager.reach_final();
            }
            
        }
    }
}

