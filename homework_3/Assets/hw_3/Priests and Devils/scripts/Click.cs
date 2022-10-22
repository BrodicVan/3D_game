using UnityEngine;

public class Click : MonoBehaviour
{
    BoatModel boat;
    RoleModel role;
    IUserAction action;
    // Start is called before the first frame update
    void Start()
    {
        action = SSDirector.get_director().get_controller() as IUserAction;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void set_boat(BoatModel b)
    {
        boat = b;
    }

    public void set_role(RoleModel r)
    {
        role = r;
    }
    public BoatModel get_boat()
    {
        return boat;
    }

    public RoleModel get_role()
    {
        return role;
    }
    public void OnMouseDown()
    {
        if(boat==null&&role==null) return;
        if(boat!=null) 
        {
            action.move_boat();
        }
        else 
        {
            action.move_role(role);
        }
    }
}
