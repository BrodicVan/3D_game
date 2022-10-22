using UnityEngine;

public interface IUserAction 
{
    void restart();
    void move_boat();
    void move_role(RoleModel r);
    void start_to_game();
}

public enum Game_Status
{
    Start,
    Gameing,
    Success,
    Loss 
};

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    private Game_Status status;
    // Start is called before the first frame update
    void Start()
    {
        status = Game_Status.Start;
    }

    // Update is called once per frame
    void OnGUI()
    {
        if(status==Game_Status.Start)
        {
            GUIStyle button_style = new GUIStyle();
            button_style.alignment=TextAnchor.MiddleCenter;
            button_style.fontSize=25;
            button_style.normal.textColor=Color.black;

            GUIStyle title_style = new GUIStyle();
            title_style.alignment=TextAnchor.MiddleCenter;
            title_style.fontSize=50;
            title_style.normal.textColor=Color.black;

            GUI.Label(new Rect(Screen.width/2-200f,Screen.height/2-100f,400f,100f),"Priests and Devils",title_style);

            if(GUI.Button(new Rect(Screen.width/2-50f,Screen.height/2+20f,100f,100f),"开    始",button_style))
            {
                status = Game_Status.Gameing;
                action.start_to_game();
            }
        }
        if(status==Game_Status.Success)
        {
            GUIStyle button_style = new GUIStyle();
            button_style.alignment=TextAnchor.MiddleCenter;
            button_style.fontSize=25;
            button_style.normal.textColor=Color.black;
            GUIStyle title_style = new GUIStyle();
            title_style.alignment=TextAnchor.MiddleCenter;
            title_style.fontSize=50;
            title_style.normal.textColor=Color.red;

            GUI.Label(new Rect(Screen.width/2-200f,Screen.height/2-100f,400f,100f),"游戏胜利",title_style);
            if(GUI.Button(new Rect(Screen.width/2-50f,Screen.height/2+20f,100f,100f),"重新开始",button_style))
            {
                status = Game_Status.Gameing;
                action.restart();
            }
        }
        if(status==Game_Status.Loss)
        {
            GUIStyle button_style = new GUIStyle();
            button_style.alignment=TextAnchor.MiddleCenter;
            button_style.fontSize=25;
            button_style.normal.textColor=Color.black;

            GUIStyle title_style = new GUIStyle();
            title_style.alignment=TextAnchor.MiddleCenter;
            title_style.fontSize=50;
            title_style.normal.textColor=Color.red;

            GUI.Label(new Rect(Screen.width/2-200f,Screen.height/2-100f,400f,100f),"游戏失败",title_style);
            if(GUI.Button(new Rect(Screen.width/2-50f,Screen.height/2+20f,100f,100f),"重新开始",button_style))
            {
                status = Game_Status.Gameing;
                action.restart();
            }
        }
    }
    public void set_action(IUserAction a)
    {
        action = a;
    }
    public void set_status(Game_Status s)
    {
        status =  s;
    }
    public Game_Status get_status()
    {
        return status;
    }
}
