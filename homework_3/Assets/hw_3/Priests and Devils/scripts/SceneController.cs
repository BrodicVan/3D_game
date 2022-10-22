using UnityEngine;

public class SceneController : MonoBehaviour,ISceneController,IUserAction
{
    GameObject water;
    LandModel left_land,right_land;
    BoatModel boat;
    RoleModel[] priests;
    RoleModel[] devils;
    UserGUI gui;
    Camera camera;
    Light light;
    void Awake()
    {
        SSDirector director = SSDirector.get_director();
        director.set_controller(this);
        
        load_resources();
    }
    public void load_resources()
    {
        load_non_entity();
        load_entity();
    }
    private void load_non_entity()
    {
        //初始化光源
        light = Instantiate(new GameObject(),new Vector3(0,20,15),Quaternion.Euler(20,20,0)).AddComponent<Light>();
        light.type = LightType.Directional;

        // 初始化相机
        camera = Instantiate(new GameObject(),new Vector3(0,20,-15),Quaternion.identity).AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 10;
        camera.gameObject.AddComponent<Move>();
        Skybox sky = camera.gameObject.AddComponent<Skybox>();
        sky.material = Resources.Load<Material>("hw_3/sky");

        // 初始化GUI
        gui = camera.gameObject.AddComponent<UserGUI>();
        gui.set_action(this);
        gui.set_status(Game_Status.Start);
    }
    private void load_entity()
    {
        // 初始化水
        water = Instantiate(Resources.Load("hw_3/water") as GameObject);
        water.transform.position = new Vector3(0,2,0);
        

        // 初始化左地
        GameObject left_land_object = Resources.Load("hw_3/land") as GameObject;
        left_land_object.transform.position = new Vector3(-13,3,0);
        left_land = new LandModel(Instantiate(left_land_object),0);
        // left_land.set_land(left_land_object);
        // left_land.init();

        
        // 初始化右地
        GameObject right_land_object = Resources.Load("hw_3/land") as GameObject;
        right_land_object.transform.position = new Vector3(13,3,0);
        right_land = new LandModel(Instantiate(right_land_object),1);
        // right_land.set_land(right_land_object);
        // right_land.init();
        

        // 初始化船
        GameObject boat_object  = Resources.Load("hw_3/boat") as GameObject;
        boat_object.transform.position = new Vector3(-2,5,0);
        boat = new BoatModel(Instantiate(boat_object),5.0f);
        // boat.set_boat(boat_object);
        // boat.init();
        

        // 初始化神父和恶魔
        priests = new RoleModel[3];
        devils = new RoleModel[3];
        
        for(int i = 0; i < 3; i++)
        {
            RoleModel priest,devil;

            GameObject priest_object = Resources.Load("hw_3/priest") as GameObject;

            priest_object.transform.position = (LandModel.priest_l_l + new Vector3(2*i,0,0));
            priest = new RoleModel(this,Instantiate(priest_object),0,0,i,10.0f);

            left_land.enland(priest);
            priests[i] = priest;
            
            GameObject devil_object = Resources.Load("hw_3/devil") as GameObject;
            devil_object.transform.position = (LandModel.devil_l_l + new Vector3(2*i,0,0));
            devil = new RoleModel(this,Instantiate(devil_object),0,1,i,10.0f);
            left_land.enland(devil);
            devils[i] = devil;
        }
    }
    

    public void move_boat()
    {
        if(gui.get_status()!=Game_Status.Gameing) return;
        boat.leave();
    }
    public void move_role(RoleModel role)
    {
        if(gui.get_status()!=Game_Status.Gameing) return;
        int seat = role.get_seat();
        if(seat==-1)// 在岸上
        {
            if(role.get_land_loc()==0)
                role.leave(boat,left_land);
            else
                role.leave(boat,right_land);
        }
        else// 在船上
        {
            if(boat.get_pos()==0)
                role.leave(boat,left_land);
            else
                role.leave(boat,right_land);
        }
        
    }
    public void start_to_game()
    {
        Move m = camera.gameObject.GetComponent<Move>();
        m.move_to(new Vector3(0,8,-10),5f);
    }

    public void restart()
    {
        Destroy(water);
        Destroy(left_land.get_land());
        Destroy(right_land.get_land());
        Destroy(boat.get_boat(),0.2f);
        for(int i = 0; i < 3;i++)
        {
            Destroy(priests[i].get_role());
            Destroy(devils[i].get_role());
        }
        load_entity();
        gui.set_status(Game_Status.Gameing);
    }

    public Game_Status check_game_status()
    {
        if(right_land.get_devil_num()==3 && right_land.get_priest_num()==3) return Game_Status.Success;
        int[] boat_counts = new int[2];// 0——神父  1——恶魔
        int[] left_counts = new int[2];
        int[] right_counts = new int[2];
        
        for(int i = 0; i < 2; i++)
        {
            boat_counts[i] = 0;
            left_counts[i] = 0;
            right_counts[i] = 0;
        }

        for(int i = 0; i < 2; i++)
        {
            if(boat.get_passenger(i)==null) continue;
            boat_counts[boat.get_passenger(i).get_type()] += 1;
        }   

        if(boat.get_pos()==0)// 船在左边
        { 
            left_counts[0] += boat_counts[0];
            left_counts[1] += boat_counts[1];
        }
        else
        {
            right_counts[0] += boat_counts[0];
            right_counts[1] += boat_counts[1];
        }
        
        left_counts[0] += left_land.get_priest_num();
        left_counts[1] += left_land.get_devil_num();

        right_counts[0] += right_land.get_priest_num();
        right_counts[1] += right_land.get_devil_num();

        if((left_counts[1]>left_counts[0] && left_counts[0]!=0) || (right_counts[1]>right_counts[0] && right_counts[0]!=0))
        {
            return Game_Status.Loss;
        } 
        
        return Game_Status.Gameing;
    }

    public void update_game_status()
    {
        gui.set_status(check_game_status());
    }
}
