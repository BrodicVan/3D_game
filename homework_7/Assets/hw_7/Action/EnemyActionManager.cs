using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class EnemyActionManager : ActionManager
    {
        Patrol patrol;
        public void run_enemy(GameObject enemy)
        {
            int area_id = enemy.GetComponent<EnemyData>().enemy_area;
                // Debug.LogFormat("area_id:{0}",area_id);
            patrol = Patrol.get_action(new Vector3(0,0,(area_id-2)*100+50));
            run_action(enemy,patrol,this);
        }
        public void destroy_all()
        {
            foreach(KeyValuePair<int,BasicAction>  kv in actions)
            {
                BasicAction action = kv.Value;
                action.destroy = true;
                action.enbale = false;
            }   
        }
    }
}

