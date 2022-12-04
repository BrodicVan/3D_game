using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_6
{
    public class PhyActionManager : PhySSActionManager, ISSActionCallback
    {
        public RoundController controller;
        // Start is called before the first frame update
        protected new void Start()
        {
            controller = SSDirector.get_instance().current_controller as RoundController;
        }

        // Update is called once per frame
        protected new void Update()
        {
            if(controller.get_game_status()==1)// 游戏运行中才更新物体状态
                base.Update();
        }
        protected new void FixedUpdate()
        {
            if(controller.get_game_status()==1)// 游戏运行中才更新物体位置
                base.FixedUpdate();
        }
        public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed,
                    int intParam = 0, string strParam = null, Object objectParam = null )
        {
            if(source is PhyDiskFly)
            {
                controller.free_disk(source.game_object);
            }
        }

    }
}