using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_4
{
    public class SceneController : MonoBehaviour,ISceneController
    {
        GameObject stone_origin;
        GameObject blood_origin;
        GameObject land_origin;
        GameObject role;
        float left_time;
        // Camera camera;
        float time1,time2;
        private MyGUI myGUI;
        void Awake()
        {
            stone_origin = Resources.Load("hw_4/stone") as GameObject;
            blood_origin = Resources.Load("hw_4/blood") as GameObject;
            Instantiate(Resources.Load("hw_4/start") as GameObject,new Vector3(30,0,8.5f),Quaternion.Euler(0,0,0));
            Instantiate(Resources.Load("hw_4/end") as GameObject,new Vector3(-30,0,8.5f),Quaternion.Euler(0,0,0));
            Instantiate(Resources.Load("hw_4/inter") as GameObject,new Vector3(0,0,8.5f),Quaternion.Euler(0,0,0));
            role = Instantiate(Resources.Load("hw_4/MuChen") as GameObject,new Vector3(30,1.5f,8.5f),Quaternion.Euler(0,0,0));  
            role.AddComponent<PlayControl>().set_controller(this);
            role.GetComponent<PlayControl>();
            time1 = 0f;
            time2 = 0f;
            myGUI = new GameObject().AddComponent<MyGUI>();
            myGUI.set_controller(this);
            left_time = 200f;

            for(int x = 0; x < 50;x += 4)
            {
                for(int i = -1; i < 2;i++)
                {
                    GameObject new_stone = Instantiate(stone_origin,new Vector3(x,0,i*4),Quaternion.Euler(0,0,0));

                    float random_size = Random.Range(2f,3f);
                    new_stone.transform.localScale = new Vector3(random_size,1,random_size);
                    StoneAction stone_action = new_stone.AddComponent<StoneAction>();
                    stone_action.set_destination(stone_action.gameObject.transform.position + new Vector3(50-x,0,0));
                    stone_action.set_speed(new Vector3(1f,0,0));
                    
                    float blood_rate = Random.Range(0f,1f);
                    if(blood_rate>=0.9f)
                    {
                        GameObject new_blood = Instantiate(blood_origin,new Vector3(0,0,i*4),Quaternion.Euler(0,0,0));
                        new_blood.transform.parent = new_stone.transform;
                        new_blood.transform.localPosition = new Vector3(0,0.5f,0);
                        Click click = new_blood.AddComponent<Click>();
                        click.set_controller(this);
                    }
                }
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            
                
        }

        // Update is called once per frame
        void Update()
        {
            left_time -= Time.deltaTime;
            if(left_time<=0)
                reset();
            time1 += Time.deltaTime;
            time2 += Time.deltaTime;
            if(time1>4)
            {
                time1 = 0f;
                for(int i = -1; i < 2;i++)
                {
                    GameObject new_stone = Instantiate(stone_origin,new Vector3(0,0,i*4),Quaternion.Euler(0,0,0));

                    float random_size = Random.Range(2f,3f);
                    new_stone.transform.localScale = new Vector3(random_size,1,random_size);
                    StoneAction stone_action = new_stone.AddComponent<StoneAction>();
                    stone_action.set_destination(stone_action.gameObject.transform.position + new Vector3(50,0,0));
                    stone_action.set_speed(new Vector3(1f,0,0));
                    
                    float blood_rate = Random.Range(0f,1f);
                    if(blood_rate>=0.9f)
                    {
                        GameObject new_blood = Instantiate(blood_origin,new Vector3(0,0,i*4),Quaternion.Euler(0,0,0));
                        new_blood.transform.parent = new_stone.transform;
                        new_blood.transform.localPosition = new Vector3(0,0.5f,0);
                        Click click = new_blood.AddComponent<Click>();
                        click.set_controller(this);
                    }
                }
            }
            if(time2>2)
            {
                time2 = 0f;
                for(int i = -1; i < 2;i++)
                {
                    float stone_rate = Random.Range(0f,1f);
                    if(stone_rate>=0.5f)
                    {
                        float blood_rate = Random.Range(0f,1f);
                        GameObject new_stone = Instantiate(stone_origin,new Vector3(-40,0,i*4) + new Vector3(0,0,17),Quaternion.Euler(0,0,0));

                        float random_size = Random.Range(2f,3f);
                        new_stone.transform.localScale = new Vector3(random_size,1,random_size);
                        StoneAction stone_action = new_stone.AddComponent<StoneAction>();
                        stone_action.set_destination(stone_action.gameObject.transform.position + new Vector3(50,0,0));
                        stone_action.set_speed(new Vector3(2f,0,0));

                        if(blood_rate>=0.7f)
                        {
                            GameObject new_blood = Instantiate(blood_origin,new Vector3(0,0,i*4),Quaternion.Euler(0,0,0));
                            new_blood.transform.parent = new_stone.transform;
                            new_blood.transform.localPosition = new Vector3(0,0.5f,0);
                            Click click = new_blood.AddComponent<Click>();
                            click.set_controller(this);
                        } 
                    }
                    
                }
            }
        }

        public void collect(GameObject stone)
        {
            if(Vector3.Distance(role.transform.position,stone.transform.position)<2f)
            {
                Destroy(stone);
                role.GetComponent<PlayControl>().add_blood();
            }
                
        }

        public int get_blood_num()
        {
            return role.GetComponent<PlayControl>().get_blood_num();
        }
        public bool if_double()
        {
            return role.GetComponent<PlayControl>().get_blood_num()>=3;
        }

        public bool if_dash()
        {
            return role.GetComponent<PlayControl>().get_blood_num()>=10;
        }

        public int get_pp()
        {
            return role.GetComponent<PlayControl>().get_pp();
        }
        public void reset()
        {
            left_time = 200f;
            role.GetComponent<PlayControl>().reset();
        }

        public int get_left_time()
        {
            return (int)left_time;
        }
    }
}

