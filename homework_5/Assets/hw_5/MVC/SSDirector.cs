using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_5
{
    public interface ISceneController
    {
        
    }
    public class SSDirector : System.Object
    {
        private static SSDirector _instance;
        public ISceneController current_controller{get;set;}
        public static SSDirector get_instance()
        {
            if(_instance==null)
                _instance = new SSDirector();
            return _instance;
        }
    }
}