using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_4
{
    public partial interface ISceneController
    {
        void collect(GameObject b);
    }
    public class Click : MonoBehaviour
    {
        ISceneController controller;    
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void OnMouseDown()
        {
            controller.collect(this.gameObject);
        }

        public void set_controller(ISceneController controller)
        {
            this.controller = controller;
        }
    }    
}

