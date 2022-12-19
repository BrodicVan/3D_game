using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class ActionManager : MonoBehaviour,IActionCallback
    {
        protected Dictionary<int,BasicAction> actions = new Dictionary<int, BasicAction>();
        private List<BasicAction> add_list = new List<BasicAction>();
        private List<int> delete_list = new List<int>();
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            foreach(BasicAction action in add_list)
            {
                actions[action.GetInstanceID()] = action;
            }
            add_list.Clear();
            // Debug.Log(actions.Count);
            foreach(int k in actions.Keys)
            {
                
            }
            foreach(KeyValuePair<int,BasicAction>  kv in actions)
            {
                BasicAction action = kv.Value;
                if(action.destroy)
                {
                    delete_list.Add(kv.Key);
                }
                else if(action.enbale)
                {
                    action.Update();
                }
            }   
            foreach(int key in delete_list)
            {
                if(!actions.ContainsKey(key))
                {
                    continue;
                }
                BasicAction action = actions[key];
                actions.Remove(key);
                Object.Destroy(action);
            }
            delete_list.Clear(); 
        }
        public void run_action(GameObject game_Object,BasicAction action,IActionCallback callback)
        {
            action.callback = callback;
            action.game_object = game_Object;
            action.transform = game_Object.transform;
            actions[action.GetInstanceID()] = action;
            action.destroy = false;
            action.enbale = true;
            
            add_list.Add(action);
            action.Start();
            
        }

        public void ActionEvent(BasicAction source, int intParam = 0, GameObject objectParam = null)
        {

            if(intParam == 0)
            {
                int area_id = objectParam.GetComponent<EnemyData>().enemy_area;
                
                //侦察兵按照初始位置开始继续巡逻
                Patrol patrol = Patrol.get_action(new Vector3(0,0,(area_id-2)*100+50));
                this.run_action(objectParam, patrol, this);
            }
            else
            {
                GameObject player = objectParam.GetComponent<EnemyData>().player;
                //侦查兵跟随玩家
                FollowPlayer follow = FollowPlayer.get_action(player);
                this.run_action(objectParam, follow, this);
                
            }
        }
    }
}

