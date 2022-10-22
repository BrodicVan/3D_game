using UnityEngine;

public class LandModel:System.Object
{
    int pos;// 0——左  1——右
    RoleModel[] priests;// 从里到外分别是0、1、2
    RoleModel[] devils;// 从里到外分别是0、1、2
    GameObject land;
    // 静态成员，看做常量，便于调整
    public static Vector3 priest_l_l = new Vector3(-11,6,0);
    public static Vector3 devil_l_l = new Vector3(-19,6,0);
    public static Vector3 priest_r_r = new Vector3(11,6,0);
    public static Vector3 devil_r_r = new Vector3(19,6,0);

    public LandModel(GameObject l,int p)
    {
        land = l;
        pos = p;
        priests = new RoleModel[3];
        devils = new RoleModel[3];
        for(int i = 0; i < 3;i++)
        {
            priests[i] = null;
            devils[i] = null;
        }
    }

    void set_pos(int p) {pos=p;}
    int get_pos() {return pos;}

    public GameObject get_land()
    {
        return land;
    }
    public int get_priest_idx()
    {
        for(int i = 0; i < 3;i++)
        {
            if(priests[i]==null)
                return i;
                // return (LandModel.priest_l_l + new Vector3(2*i,0,0));
        }
        return -1;
    }

    public int get_priest_num()
    {
        int count = 0;
        for(int i = 0; i < 3;i++)
        {
            if(priests[i]!=null)
                count += 1;
                // return (LandModel.priest_l_l + new Vector3(2*i,0,0));
        }
        return count;
    }

    public int get_devil_idx()
    {
        for(int i = 0; i < 3;i++)
        {
            if(devils[i]==null)
                return i;
                // return (LandModel.devil_l_l + new Vector3(2*i,0,0));
        }
        return -1;
    }

    public int get_devil_num()
    {
        int count = 0;
        for(int i = 0; i < 3;i++)
        {
            if(devils[i]!=null)
                count += 1;
                // return (LandModel.priest_l_l + new Vector3(2*i,0,0));
        }
        return count;
    }
    
    public int get_idx(RoleModel role)
    {
        if(role.get_type()==0) return get_priest_idx();
        else return get_devil_idx();
    }
    public Vector3 enland(RoleModel role)
    {
        
        int loc;
        if(role.get_type()==0)// 神父
        {
            loc = get_priest_idx();
            priests[loc] = role;
            return pos==0?(LandModel.priest_l_l + new Vector3(2*loc,0,0)):(LandModel.priest_r_r - new Vector3(2*loc,0,0));
        }
        else// 恶魔
        {
            loc = get_devil_idx();
            devils[loc] = role;
            return pos==0?(LandModel.devil_l_l + new Vector3(2*loc,0,0)):(LandModel.devil_r_r - new Vector3(2*loc,0,0));
        }
    }

    public void unland(RoleModel role,int loc)
    {
        if(role.get_type()==0)
        {
            priests[loc] = null;
        }
        else
        {
            devils[loc] = null;
        }
    }
}
