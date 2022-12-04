using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_6

{
    public class PhyDiskFly : SSAction
    {
        public Vector3 start;
        public float vx_0;// 水平初速度
        public float vy_0;// 垂直初速度
        public float dy;// 垂直加速度
        public static PhyDiskFly GetPhyDiskFly(Vector3 start,float vx, float vy, float dy)
        {
            PhyDiskFly action = ScriptableObject.CreateInstance<PhyDiskFly>();
            action.start = start;
            action.vx_0 = vx;
            action.vy_0 = vy;
            action.dy = dy;
            return action;
        }
        // Start is called before the first frame update
        public override void Start()
        {
            this.game_object.transform.position = start;
            Rigidbody rigidbody = game_object.GetComponent<Rigidbody>();
            if(rigidbody==null)
                rigidbody = game_object.AddComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.velocity = new Vector3(vx_0,vy_0,0);
            rigidbody.useGravity = false;
        }

        // Update is called once per frame
        public override void Update()
        {
            if(this.game_object.transform.position.y > -5 && this.game_object.transform.position.x < 40f && this.game_object.activeSelf)
            {
                // vy += Time.deltaTime*dy;
                // this.game_object.transform.position += Time.deltaTime*new Vector3(vx,vy,0);
                // this.game_object.transform.rotation = Quaternion.AngleAxis(Mathf.Atan(vy/vx)*180/Mathf.PI,Vector3.forward);
            }
            else// 飞碟出界或落地
            {
                this.destroy = true;// 销毁该动作
                this.callback.SSActionEvent(this);// 通知动作管理器
            }  
        }
        public override void FixedUpdate()
        {
            Rigidbody rigidbody = this.game_object.GetComponent<Rigidbody>();
            rigidbody.AddForce(0,dy,0,ForceMode.Acceleration);
            rigidbody.rotation = Quaternion.AngleAxis(Mathf.Atan(rigidbody.velocity.y/rigidbody.velocity.x)*180/Mathf.PI,Vector3.forward);
        }
    }
}
