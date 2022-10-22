using UnityEngine;



public class RoleModel:System.Object
{
    public GameObject role;
    private int type;// 0——神父  1——恶魔
    private ISceneController controller;
    private Move move;
    private Click click;
    private int seat;// 船上位置
    private int idx;// 陆地上坐标
    private int land_loc;// 陆地位置
    private float speed;
    public RoleModel(ISceneController c,GameObject r,int l_loc,int typ,int index,float s)
    {
        controller = c;
        speed = s;
        role = r;
        move = role.AddComponent<Move>() as Move;
        move.set_speed(10f);
        move.set_move_status(false);
        move.set_destination(role.transform.position);
        click = role.AddComponent<Click>() as Click;
        click.set_role(this);
        seat = -1;
        land_loc = l_loc;
        type = typ;
        idx = index;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void set_type(int t) {type = t;}
    public int get_type() {return type;}
    public void set_idx(int i) {idx = i;}
    public int get_idx() {return idx;}
    public void set_land_loc(int l) {land_loc = l;}
    public int get_land_loc() {return land_loc;}
    public void set_role(GameObject r)
    {
        role = r;
    }
    public int get_seat()
    {
        return seat;
    }
    public GameObject get_role()
    {
        return role;
    }
    
    public void leave(BoatModel boat,LandModel land)
    {
        if(move.get_move_status()) return;
        if(boat.get_move_status()) return;
        if(seat==-1)// 在岸上
        {
            // 船做好登船准备
            int tem_seat= boat.ship(this);

            if(tem_seat==-1) return;
            
            // 地做好离地准备
            land.unland(this,idx);

            // 切换位置状态
            seat = tem_seat;
            land_loc = -1;
            idx = -1;
            move.move_to(boat.get_pass_position(seat),speed);
        }
        else// 在船上
        {
            int boat_loc = boat.get_pos();
            int tar_idx = land.get_idx(this);

            // 船做好下船准备
            boat.unship(this,seat);

            // 地做好登陆准备
            Vector3 tar_position = land.enland(this);// 会修改数组


            // 切换位置状态
            seat = -1;
            land_loc = boat_loc;
            idx = tar_idx;
            move.move_to(tar_position,speed);
            
        }
    }
}
