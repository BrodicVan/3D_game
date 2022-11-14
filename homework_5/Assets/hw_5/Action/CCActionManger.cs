using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_5
{
    public class CCActionManger : SSActionManager, ISSActionCallback
    {
        public RoundController controller;
        // Start is called before the first frame update
        protected new void Start()
        {
            controller = SSDirector.get_instance().current_controller as RoundController;
            controller.action_manager = this;

        }

        // Update is called once per frame
        protected new void Update()
        {
            if(controller.get_game_status()==1)// 游戏运行中才更新物体位置
                base.Update();
        }
        public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed,
                    int intParam = 0, string strParam = null, Object objectParam = null )
        {
            if(source is DiskFly)
            {
                controller.free_disk(source.game_object);
            }
        }

    }
}

