using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_6
{
    public class ActionAdapter : System.Object,IActionManger
    {
        RoundController controller;
        PhyActionManager phy_manager;

        CCActionManger cc_manager;

        public ActionAdapter()
        {
            controller = SSDirector.get_instance().current_controller as RoundController;            
            phy_manager = new GameObject().AddComponent<PhyActionManager>();
            phy_manager.controller= controller;
            cc_manager = phy_manager.gameObject.AddComponent<CCActionManger>();
            cc_manager.controller = controller;
        }

        public int get_action_num()
        {
            return phy_manager.get_action_num() + cc_manager.get_action_num();
        }

        public void play_disk(GameObject disk,bool if_phy, Vector3 start, float vx, float vy, float dy)
        {
            if(if_phy)
            {
                phy_manager.RunAction(disk,PhyDiskFly.GetPhyDiskFly(start,vx,vy,dy),phy_manager);
            }
            else
            {
                // Debug.Log("cc");
                cc_manager.RunAction(disk,DiskFly.GetDiskFly(start,vx,vy,dy),cc_manager);
            }
        }
    }
}

