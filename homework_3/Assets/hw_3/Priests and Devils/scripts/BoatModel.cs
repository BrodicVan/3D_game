using UnityEngine;

public class BoatModel:System.Object
{
    public GameObject boat;
    private ISceneController cur_controller;
    private int pos;// 0——左  1——右
    private Move move;
    private Click click;
    private Check_Wait check_wait_enship;
    private Check_Wait check_wait_unship;
    RoleModel[] passengers;// 0——左  1——右
    private int[] waits;// 是否有等待，是——1  否——0
    private float speed;

    // 静态成员，看做常量，便于调整
    static Vector3 seat_left = new Vector3(-1.5f,1f,0);
    static Vector3 seat_right = new Vector3(1.5f,1f,0);
    static Vector3 stop_left = new Vector3(-2,5,0);
    static Vector3 stop_right = new Vector3(2,5,0);

    public BoatModel(GameObject b,float s)
    {
        speed = s;
        boat = b;
        move = boat.AddComponent<Move>();
        move.set_speed(speed);
        move.set_move_status(false);
        move.set_destination(boat.transform.position);
        pos = 0;
        passengers = new RoleModel[2];
        passengers[0] = null;
        passengers[1] = null;
        
        click = boat.AddComponent<Click>();
        click.set_boat(this);

        waits = new int[2];
        waits[0] = 0;
        waits[1] = 0;

        check_wait_enship = boat.AddComponent<Check_Wait>();
        check_wait_enship.set_boat(this);
        check_wait_enship.set_type(0);

        check_wait_unship = boat.AddComponent<Check_Wait>();
        check_wait_unship.set_boat(this);
        check_wait_unship.set_type(1);
    }

    

    public void set_pos(int p) {pos=p;}
    public int get_pos() {return pos;}
    public void set_boat(GameObject b)
    {
        boat = b;
    }
    public GameObject get_boat()
    {
        return boat;
    }
    
    int get_pass_num()
    {
        int count = 0;
        for(int i = 0; i < 2; i++)
            if(passengers[i]!=null)
                count++;
        return count;
    }

    public bool get_move_status()
    {
        return move.get_move_status();
    }

    public int ship(RoleModel role)// 返回座位
    {
        int seat = -1;
        if(move.get_move_status()) // 船在动
        {
            return seat;
        }
        if(role.get_land_loc() != pos) // 船和人不在同一边
        {
            return seat;
        }
        if(passengers[0]==null)
            seat = 0;
        else if(passengers[1]==null)
            seat = 1;
        else 
        {
            return seat;
        }
        // role.transform.SetParent(this.gameObject.transform);// 设置父子关系，将role绑定到船上
        // role.transform.localPosition = seat==0?BoatModel.seat_left:BoatModel.seat_right;// 设置相对位置
        passengers[seat] = role;// 设置位置上的乘客

        waits[seat]=1;
        start_wait(seat);
        return seat;
    }

    public void unship(RoleModel role,int seat)// 未改变位置
    {
        if(passengers[seat]!=role) return;

        int wait_idx = check_wait_unship.get_empty();
        check_wait_unship.set_wait(wait_idx);
        Vector3 des;
        if(get_pos()==0)
        {
            des = stop_left - new Vector3(boat.transform.localScale.x/2,0,0) + new Vector3(0,1f,0);
        }
        else
        {
            des = stop_right + new Vector3(boat.transform.localScale.x/2,0,0) + new Vector3(0,1f,0) ;
        }
        // Debug.Log(des);
        check_wait_unship.set_role(wait_idx,role);
        check_wait_unship.set_destination(wait_idx,des);

        passengers[seat] = null;
        role.get_role().transform.SetParent(null);// 删除父子关系
        // waits[seat] = 0;
    }

    int get_wait_num()
    {
        return waits[0] + waits[1];
    }
    public void leave()
    {
        if(check_wait_enship.get_wait_num()>0) return;
        if(check_wait_unship.get_wait_num()>0) 
        {
            Debug.Log(check_wait_unship.get_wait_num());
            return;
        }
        if(get_pass_num()==0)
        {
            return;
        } 
        if(move.get_move_status()) 
        {
            return;
        }
        if(pos==0)
        {
            move.move_to(stop_right,speed);
            pos = 1;
        }
        else
        {
            move.move_to(stop_left,speed);
            pos = 0;
        }
    }

    public Vector3 get_pass_position(int seat)
    {
        return boat.transform.position + (seat==0?seat_left:seat_right);
    }
    public RoleModel get_passenger(int seat)
    {
        return passengers[seat];
    }

    public void start_wait(int seat)
    {
        Vector3 des = pos==0?stop_left + (seat==0?seat_left:seat_right): stop_right + (seat==0?seat_left:seat_right);
        waits[seat] = 1;
        check_wait_enship.set_destination(seat,des);
        check_wait_enship.set_wait(seat);
        check_wait_enship.set_role(seat,passengers[seat]);
    }
    public void end_wait(int seat,int type)// type: 0——上船 1——下船
    {
        if(type==0)
        {
            passengers[seat].get_role().transform.parent = boat.transform;
        waits[seat] = 0;
        }
        
    }
   
}
