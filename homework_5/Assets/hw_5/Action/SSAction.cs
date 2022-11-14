using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_5
{
    public enum SSActionEventType:int{Started, Completed}
    public interface ISSActionCallback
    {
        void SSActionEvent(SSAction source,
            SSActionEventType events = SSActionEventType.Completed,
            int intParam = 0,
            string strParam = null,
            Object objectParam = null);
    }
    public class SSAction : ScriptableObject
    {
        public bool enable = true;
        public bool destroy = false;

        public GameObject game_object{get;set;}
        // public Transform transform{get;set;}
        public ISSActionCallback callback{get;set;}

        protected SSAction() {}
        // Start is called before the first frame update
        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}

