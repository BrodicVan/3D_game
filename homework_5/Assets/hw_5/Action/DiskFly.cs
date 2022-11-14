using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_5
{
    public class DiskFly : SSAction
    {
        public Vector3 start;
        public float vx;// 水平速度
        public float vy;// 垂直速度
        public float dy;// 垂直加速度
        public static DiskFly GetDiskFly(Vector3 start,float vx, float vy, float dy)
        {
            DiskFly action = ScriptableObject.CreateInstance<DiskFly>();
            action.start = start;
            action.vx = vx;
            action.vy = vy;
            action.dy = dy;
            return action;
        }
        // Start is called before the first frame update
        public override void Start()
        {
            this.game_object.transform.position = start;
            
        }

        // Update is called once per frame
        public override void Update()
        {
            if(this.game_object.transform.position.y > -5 && this.game_object.transform.position.x < 35f && this.game_object.active)
            {
                vy += Time.deltaTime*dy;
                this.game_object.transform.position += Time.deltaTime*new Vector3(vx,vy,0);
                this.game_object.transform.rotation = Quaternion.AngleAxis(Mathf.Atan(vy/vx)*180/Mathf.PI,Vector3.forward);
            }
            else// 飞碟出界或落地
            {
                this.destroy = true;// 销毁该动作
                this.callback.SSActionEvent(this);// 通知动作管理器
            }  
        }
    }
}

