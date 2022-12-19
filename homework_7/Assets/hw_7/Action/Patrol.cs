using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class Patrol : BasicAction
    { 
        Vector3 origin;
        EnemyData data;
        Vector3 target;
        float speed = 15;
        int length;
        int target_idx;
        int idx_num;
        int[] dx = new int[]{1,-1,-1,1};
        int[] dz = new int[]{1,1,-1,-1};

        public static Patrol get_action(Vector3 origin)
        {
            Patrol action = CreateInstance<Patrol>();
            action.origin = origin;
            return action;
        }
        // Start is called before the first frame update
        override public void Start()
        {
            data = game_object.GetComponent<EnemyData>();
            length = Random.Range(20,40);
            target_idx = Random.Range(0,4);
            target = origin + new Vector3(dx[target_idx],0f,dz[target_idx])*length;
            idx_num = 1;
            // Debug.LogFormat("cur:{0} target:{1} origin:{2}",transform.position,target,origin);
        }

        // Update is called once per frame
        override public void Update()
        {   
            
            
            // Debug.LogFormat("enemy:{0},player:{1}",data.enemy_area,data.player_area);
            if(data.enemy_area == data.player_area && data.follow_player)
            {
                callback.ActionEvent(this,1,game_object);
                this.destroy = true;
                this.enbale = false;
                Debug.Log("Patrol End");
                return;
            }
            else
            {
                change_target();
                transform.position = Vector3.MoveTowards(transform.position,target,(speed+data.enemy_area*5)*Time.deltaTime);
                transform.LookAt(target);
            }
            
        }
        void change_target()
        {
            if(Vector3.Distance(transform.position,target) < 1)
            {
                
                if(idx_num==4)
                {
                    idx_num = 0;
                    target_idx = Random.Range(0,4);
                }
                else
                {
                    idx_num += 1;
                    target_idx = (target_idx+1)%4;
                }                    
                length = Random.Range(20,40);
                target = origin + new Vector3(dx[target_idx],0f,dz[target_idx])*length;
            }
            
        }
    }
}
