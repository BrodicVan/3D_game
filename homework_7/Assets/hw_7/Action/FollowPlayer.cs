using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hw_7
{
    class FollowPlayer : BasicAction
    {
        GameObject player; 
        float speed = 42f;
        EnemyData data;

        public static FollowPlayer get_action(GameObject player)
        {
            FollowPlayer action = CreateInstance<FollowPlayer>();
            action.player = player;
            return action;
        }
        // Start is called before the first frame update
        override public void Start()
        {
            data = game_object.GetComponent<EnemyData>();
        }

        // Update is called once per frame
        override public void Update()
        {
            if(data.enemy_area!=data.player_area || !data.follow_player)
            {
                //玩家逃脱
                GameEventManager.player_escape();
                callback.ActionEvent(this,0,game_object);
                this.destroy = true;
                this.enbale = false;
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position,player.transform.position,speed*Time.deltaTime);
            transform.LookAt(new Vector3(player.transform.position.x,0,player.transform.position.z));
            // if(data.follow_player)
            // {
                
            // }
            // else
            // {
            //     callback.ActionEvent(this,0,game_object);
            //     this.destroy = true;
            //     this.enbale = false;
            // }
        }
    }

}

