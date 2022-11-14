using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_5
{
    public class SSActionManager : MonoBehaviour
    {
        private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
        private List<SSAction> waitingAdd = new List<SSAction>();
        private List<int> waitingDelete = new List<int>();

        // Update is called once per frame
        protected void Update()
        {
            foreach(SSAction ac in waitingAdd) actions[ac.GetInstanceID()] = ac;
            waitingAdd.Clear();

            foreach(KeyValuePair<int,SSAction> kv in actions)
            {
                SSAction ac = kv.Value;
                if(ac.destroy)// 若需要被释放
                {
                    waitingDelete.Add(ac.GetInstanceID());
                }
                else if(ac.enable)// 若可行
                {
                    ac.Update();
                }
            }
            foreach(int key in waitingDelete)
            {
                SSAction ac =actions[key];
                actions.Remove(key);
                Object.Destroy(ac);
            }
            waitingDelete.Clear();
        }
        public int get_action_num()
        {
            return actions.Count;
        } 
        public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
        {
            action.game_object = gameobject;
            // action.transform = gameobject.transform;
            action.callback = manager;
            waitingAdd.Add(action);
            action.Start();
        }

        protected void Start()
        {}

    }
}


