using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    enum ActionEventType:int
    {
        Started,Completed
    }
    enum GameStatusType: int
    {
        Ready,Run,Win,Lose
    }
    interface IDirectorSupportor
    {

    }

    interface IGUISupportor
    {
        GameStatusType get_game_status();
        void start_game();
        int get_score();
    }


    interface IActionCallback
    {
        void ActionEvent(BasicAction source,int intParam = 0,GameObject objectParam = null);
    }
    
}

