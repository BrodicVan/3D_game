using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    public class MyGUI : MonoBehaviour
    {
        IGUISupportor controller;
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnGUI()
        {
            GameStatusType game_status = controller.get_game_status();

            if(game_status==GameStatusType.Ready)
            {
                GUIStyle title_fontStyle = new GUIStyle();  
                title_fontStyle.alignment=TextAnchor.MiddleCenter;
                title_fontStyle.fontSize=50;
                title_fontStyle.normal.textColor=Color.blue;
                GUI.TextArea(new Rect(Screen.width/2-60,Screen.height/2-80,120,50),"智能巡逻兵",title_fontStyle);

                GUIStyle start_fontStyle = new GUIStyle();  
                start_fontStyle.alignment=TextAnchor.MiddleCenter;
                start_fontStyle.fontSize=40;
                start_fontStyle.normal.textColor=Color.red;
                if(GUI.Button(new Rect(Screen.width/2-60,Screen.height/2+40,120,30),"开始游戏",start_fontStyle))
                    controller.start_game();
                
            }
            else
            if(game_status == GameStatusType.Run)
            {
                GUIStyle score_fontStyle = new GUIStyle();  
                score_fontStyle.alignment=TextAnchor.MiddleCenter;
                score_fontStyle.fontSize=50;
                score_fontStyle.normal.textColor=Color.black;
                GUI.TextArea(new Rect(70,50,120,50),string.Format("分数：{0}",controller.get_score()),score_fontStyle);
            }
            else
            if(game_status==GameStatusType.Win)
            {
                string message = "游戏胜利";

                GUIStyle title_fontStyle = new GUIStyle();  
                title_fontStyle.alignment=TextAnchor.MiddleCenter;
                title_fontStyle.fontSize=50;
                title_fontStyle.normal.textColor = Color.blue;
                GUI.TextArea(new Rect(Screen.width/2-60,Screen.height/2-80,120,50),message,title_fontStyle);

                GUIStyle score_fontStyle = new GUIStyle();  
                score_fontStyle.alignment=TextAnchor.MiddleCenter;
                score_fontStyle.fontSize=30;
                score_fontStyle.normal.textColor=Color.black;
                GUI.TextArea(new Rect(Screen.width/2-60,Screen.height/2,120,50),string.Format("分数：{0}",controller.get_score()),score_fontStyle);

                GUIStyle start_fontStyle = new GUIStyle();  
                start_fontStyle.alignment=TextAnchor.MiddleCenter;
                start_fontStyle.fontSize=40;
                start_fontStyle.normal.textColor=Color.red;
                if(GUI.Button(new Rect(Screen.width/2-60,Screen.height/2+60,120,30),"重新开始",start_fontStyle))
                    controller.start_game();
            }
            else
            if(game_status==GameStatusType.Lose)
            {
                string message = "游戏失败";

                GUIStyle title_fontStyle = new GUIStyle();  
                title_fontStyle.alignment=TextAnchor.MiddleCenter;
                title_fontStyle.fontSize=50;
                title_fontStyle.normal.textColor = Color.red;
                GUI.TextArea(new Rect(Screen.width/2-60,Screen.height/2-80,120,50),message,title_fontStyle);

                GUIStyle start_fontStyle = new GUIStyle();  
                start_fontStyle.alignment=TextAnchor.MiddleCenter;
                start_fontStyle.fontSize=40;
                start_fontStyle.normal.textColor=Color.red;
                if(GUI.Button(new Rect(Screen.width/2-60,Screen.height/2+40,120,30),"重新开始",start_fontStyle))
                    controller.start_game();
            }
        }


        internal void set_support(IGUISupportor supportor)
        {
            controller = supportor;
        }
    }
}

