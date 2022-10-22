using UnityEngine;

public class Check_Wait : MonoBehaviour
{
    private int type;// 0——上船 1——下船
    private int[] waits;
    private BoatModel boat;
    private Vector3[] destinations;
    private RoleModel[] roles;
    public void set_boat(BoatModel b)
    {
        boat = b;
    }
    public void set_destination(int seat,Vector3 des)
    {
        destinations[seat] = des;
    }
    public void set_wait(int w_s)
    {
        // Debug.LogFormat("set_wait:{0}",w_s);
        waits[w_s] = 1;
    }
    public void set_role(int seat,RoleModel role)
    {
        roles[seat] = role;
    }
    public void set_type(int t)
    {
        type = t;
    }
    public int get_wait_num()
    {
        return waits[0] + waits[1];
    }
    public int get_empty()
    {
        if(waits[0]==0) return 0;
        else return 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        destinations = new Vector3[2];
        roles = new RoleModel[2];
        waits = new int[2];
        waits[0] = 0;
        waits[1] = 0;
    }
    // Update is called once per frame
    void Update()
    {
        // Debug.LogFormat("waits[0]:{0}",waits[0]);
        // Debug.LogFormat("waits[0]:{0}",waits[1]);
        if(waits[0] ==1 )// 在等0号位
        {
            // GameObject wait_object = boat.get_passenger(0).get_role();// 所等乘客的位置
            GameObject wait_object = roles[0].get_role();    
            float dis = Vector3.Distance(wait_object.transform.position,destinations[0]);
            if(dis <= 0.05f)
            {
                boat.end_wait(0,type);
                waits[0] = 0;
                roles[0] = null;
            }           
        }
        if(waits[1] == 1 )// 在等1号位
        {
            // GameObject wait_object = boat.get_passenger(1).get_role();// 所等乘客的位置
            GameObject wait_object = roles[1].get_role();      
            float dis = Vector3.Distance(wait_object.transform.position,destinations[1]);

            if(dis <= 0.05f)
            {
                boat.end_wait(1,type);
                waits[1] = 0;
                roles[1] = null;
            }           
        }
    }
}
