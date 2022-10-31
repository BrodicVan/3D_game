using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_4
{
    public partial interface ISceneController
    {
        int get_blood_num();
        bool if_double();
        bool if_dash();
    }
    public class MyGUI : MonoBehaviour
    {
        private Texture cross;
        private SceneController controller;
        // Start is called before the first frame update
        void Start()
        {
            cross = Resources.Load("hw_4/Cross") as Texture;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        void OnGUI()
        {
            GUI.DrawTexture(new Rect((Screen.width-cross.width)/2,(Screen.height-cross.height)/2,cross.width,cross.height),cross);

            int blood_num = controller.get_blood_num();

            GUIStyle blood_text = new GUIStyle();
            blood_text.alignment=TextAnchor.MiddleLeft;
            blood_text.fontSize=30;
            blood_text.normal.textColor=Color.red;
            string s1 = string.Format("血泥块数目: {0}",blood_num);
            GUI.Label(new Rect(10,10,60,60),s1,blood_text);

            if(controller.if_double())
            {
                GUIStyle double_text = new GUIStyle();
                double_text.alignment=TextAnchor.MiddleLeft;
                double_text.fontSize=30;
                double_text.normal.textColor=Color.black;
                string double_string = string.Format("二段跳");
                GUI.Label(new Rect(10,60,60,60),double_string,double_text);
            }
            if(controller.if_dash())
            {
                GUIStyle dash_text = new GUIStyle();
                dash_text.alignment=TextAnchor.MiddleLeft;
                dash_text.fontSize=30;
                dash_text.normal.textColor=Color.black;
                string dash_string = string.Format("冲刺(L_shift)");
                GUI.Label(new Rect(10,110,60,60),dash_string,dash_text);
            }         
        }

        public void set_controller(SceneController c)
        {
            controller = c;
        }
    }
}

