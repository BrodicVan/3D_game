using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class AreaCollision : MonoBehaviour
    {
        public int area_id = 0;
        SceneController controller;
        // Start is called before the first frame update
        void Start()
        {
            Director director = Director.get_director();
            // Debug.Log(director==null);
            controller = director.cur_controller as SceneController;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag =="Player")
            {
                if(controller==null)
                {
                    controller = Director.get_director().cur_controller as SceneController;
                }
                controller.play_area = area_id;
            }
            
        }
    }
}

