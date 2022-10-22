using UnityEngine;

public class Move : MonoBehaviour
{
    private Vector3 destination;// 移动目的地
    private float speed;
    private bool move_status;// false——静止  true——移动

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if(move_status && Vector3.Distance(this.gameObject.transform.position,destination)==0f)
        {
            move_status = false;
            SSDirector.get_director().get_controller().update_game_status();
        }
        if(move_status)
        {
            this.gameObject.transform.position 
                = Vector3.MoveTowards(this.gameObject.transform.position,destination,speed*Time.deltaTime);
            
        }
    }

    public void move_to(Vector3 des,float s)
    {
        speed = s;
        destination = des;
        move_status = true;
    }

    public void set_move_status(bool m) {move_status = m;}
    public bool get_move_status() {return move_status;}
    public void set_speed(float s) {speed = s;}
    public float get_speed() {return speed;}
    public void set_destination(Vector3 des) {destination = des;}
    public Vector3 get_destination() {return destination;}
}
