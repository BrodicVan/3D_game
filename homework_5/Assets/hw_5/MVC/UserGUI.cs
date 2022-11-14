using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_5
{
    public interface IUserAction
    {
        void start();
        void restart();
        int get_game_status();
        int get_score();
        int get_bullct();
        int get_shoot_status();
    }
    public class UserGUI : MonoBehaviour
    {
        IUserAction controller;
        public Texture cross;
        // Start is called before the first frame update
        void Start()
        {
            cross = Resources.Load<Texture>("hw_5/Cross2");
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnGUI()
        {
            int game_status = controller.get_game_status();
            GUI.DrawTexture(new Rect((Screen.width-cross.width)/2,(Screen.height-cross.height)/2,cross.width,cross.height),cross);
            // GUI.Box(new Rect(20f,20f,200f,200f),string.Format("分数：{0}\n子弹：{1}/45",controller.get_score(),controller.get_bullct()));

            if(game_status==0)
            {
                GUIStyle title_fontStyle = new GUIStyle();  
                title_fontStyle.alignment=TextAnchor.MiddleCenter;
                title_fontStyle.fontSize=40;
                title_fontStyle.normal.textColor=Color.blue;
                GUI.TextArea(new Rect(Screen.width/2-60,Screen.height/2-80,120,50),"打飞碟",title_fontStyle);

                GUIStyle start_fontStyle = new GUIStyle();  
                start_fontStyle.alignment=TextAnchor.MiddleCenter;
                start_fontStyle.fontSize=25;
                start_fontStyle.normal.textColor=Color.red;
                if(GUI.Button(new Rect(Screen.width/2-60,Screen.height/2+40,120,30),"开始游戏",start_fontStyle))
                    controller.start();
            }
            else if(game_status==1 || game_status==3)
            {
                GUIStyle score_fontStyle = new GUIStyle();  
                score_fontStyle.alignment=TextAnchor.MiddleLeft;
                score_fontStyle.fontSize=50;
                score_fontStyle.normal.textColor=Color.black;
                GUI.TextArea(new Rect(30,30,120,60),string.Format("得分:{0}",controller.get_score()),score_fontStyle);

                int bullet = controller.get_bullct();
                GUIStyle bullet_fontStyle = new GUIStyle();  
                bullet_fontStyle.alignment=TextAnchor.MiddleLeft;
                bullet_fontStyle.fontSize=25;
                bullet_fontStyle.normal.textColor = bullet > 15? Color.black: Color.red;
                GUI.TextArea(new Rect(Screen.width-200,200,120,70),string.Format("子弹:{0}/45\n(按R换弹)",bullet),bullet_fontStyle);
            

                GUIStyle status_fontStyle = new GUIStyle();  
                status_fontStyle.alignment=TextAnchor.MiddleLeft;
                status_fontStyle.fontSize=20;
                status_fontStyle.normal.textColor=Color.black;
                GUI.TextArea(new Rect(Screen.width-200,300,120,70),string.Format("开枪模式:{0}\n(按E切换)",controller.get_shoot_status()==0?"连发":"单发"),status_fontStyle);
                
                GUIStyle rule_fontStyle = new GUIStyle();  
                rule_fontStyle.alignment=TextAnchor.MiddleCenter;
                rule_fontStyle.fontSize=25;
                rule_fontStyle.normal.textColor=Color.black;
                GUI.TextArea(new Rect(Screen.width-200,30,120,60),
                                string.Format("得分规则\n连发: 绿/蓝/红——1/2/3\n单发: 绿/蓝/红——2/3/4",controller.get_score()),rule_fontStyle);

            }
            else if(game_status==2)
            {
                GUIStyle final_fontStyle = new GUIStyle();  
                final_fontStyle.alignment=TextAnchor.MiddleCenter;
                final_fontStyle.fontSize=40;
                final_fontStyle.normal.textColor=Color.blue;
                GUI.TextArea(new Rect(Screen.width/2-60,Screen.height/2-80,120,50),string.Format("最终得分:{0}",controller.get_score()),final_fontStyle);

                GUIStyle restart_fontStyle = new GUIStyle();  
                restart_fontStyle.alignment=TextAnchor.MiddleCenter;
                restart_fontStyle.fontSize=25;
                restart_fontStyle.normal.textColor=Color.red;
                if(GUI.Button(new Rect(Screen.width/2-60,Screen.height/2+40,120,30),"重新开始",restart_fontStyle))
                    controller.start();
            }
        }
        public void set_controller(IUserAction a)
        {
            controller = a;
        }
    }
}

