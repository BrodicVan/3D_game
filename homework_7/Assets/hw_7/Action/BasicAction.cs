using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class BasicAction: ScriptableObject
    {
        
        public bool enbale = true;
        public bool destroy = false;
        public GameObject game_object{get;set;}
        public Transform transform{get;set;}
        public IActionCallback callback{get;set;}

        public virtual void Start()
        {
            throw new NotImplementedException();
        }
        public virtual void Update()
        {
            throw new NotImplementedException();
        }
    }

    
}

