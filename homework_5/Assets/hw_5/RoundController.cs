using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hw_5
{
    public class RoundController : MonoBehaviour,IUserAction, IPlayer, ISceneController
    {
        DiskFactory disk_factory;
        private ScoreController score_controller;
        public CCActionManger action_manager{set;get;}
        private UserGUI gui;
        private PlayControl player;
        private float gaming_time;// 游戏时间
        private float waiting_time;// 回合等待时间
        private int round,trail;
        private int game_status;//0—未开始 1—游戏中 2—结束 3—回合过渡中

        // Start is called before the first frame update
        void Start()
        {
            disk_factory = Singleton<DiskFactory>.Instance;
            score_controller = new ScoreController();
            action_manager = new GameObject().AddComponent<CCActionManger>();
            gui = action_manager.gameObject.AddComponent<UserGUI>();
            gui.set_controller(this);
            game_status = 0;
            gaming_time = 0f;
            waiting_time = 0f;
            round = 1;
            trail = 1;
            SSDirector director = SSDirector.get_instance();
            director.current_controller = this;
            Instantiate<GameObject>(Resources.Load<GameObject>("hw_5/land"),new Vector3(0,0,-35),Quaternion.Euler(90,0,0));
            Instantiate<GameObject>(Resources.Load<GameObject>("hw_5/Wall"),new Vector3(0,0,0),Quaternion.Euler(90,0,0));
            Instantiate<GameObject>(Resources.Load<GameObject>("hw_5/Wall"),new Vector3(30,0,0),Quaternion.Euler(0,90,0));// 右墙
            Instantiate<GameObject>(Resources.Load<GameObject>("hw_5/Wall"),new Vector3(-30,0,0),Quaternion.Euler(0,-90,0));// 左墙
            player = Instantiate<GameObject>(Resources.Load<GameObject>("hw_5/Man"),new Vector3(0,1,-30),Quaternion.Euler(0,0,0)).AddComponent<PlayControl>();
            player.set_controller(this);
            Cursor.lockState = CursorLockMode.None;
        }

        // Update is called once per frame
        void Update()
        {
            if(game_status==0)// 未开始
            {
                
            }
            if(game_status==1)// 游戏中
            {
                game();
            }
            if(game_status==2)// 结束
            {

            }
            if(game_status==3)// 回合过渡中
            {
                waiting_time += Time.deltaTime;
                if(waiting_time > 2)
                {
                    game_status = 1;// 进入游戏中状态
                    round += 1;
                    trail = 1;
                    waiting_time = 0;
                    gaming_time = 0;
                }
            }
        }

        void create_one_disk(float small,float mid)
        {
            float y_bia = Random.Range(0f,10f);// 随机设置飞出高度
            float z_bia = Random.Range(-5f,5f);
            float sample = Random.Range(0.0f,1.0f);
            float vx = Random.Range(15f,20f);
            float vy = Random.Range(6f,7f);
            float dy = -4f;
            Vector3 start = new Vector3(-35,y_bia,z_bia);
            DiskData disk = null;
            if(sample <= small)
                disk =  disk_factory.get_disk(1);
            else if(sample <= mid)
                disk = disk_factory.get_disk(2);
            else
                disk = disk_factory.get_disk(3);
            action_manager.RunAction(disk.gameObject,DiskFly.GetDiskFly(start,vx*disk.speed,vy,dy),action_manager);
        }

        void game()
        {
            if(trail <= 10)// 一个回合还没结束
            {
                gaming_time += Time.deltaTime;    
                if(gaming_time < 2) return;
                
                if(round == 1)
                {
                    if(gaming_time>=1)
                    {
                        create_one_disk(1,1);
                        create_one_disk(1,1);
                        create_one_disk(1,1);
                        gaming_time = 0f;
                        trail += 1;
                    }
                }
                if(round == 2)
                {
                    if(gaming_time>=1)
                    {
                        create_one_disk(0.5f,1);
                        create_one_disk(0.5f,1);
                        create_one_disk(0.5f,1);
                        gaming_time = 0f;
                        trail += 1;
                    }
                }
                if(round == 3)
                {
                    if(gaming_time>=1)
                    {
                        create_one_disk(0.3f,0.8f);
                        create_one_disk(0.3f,0.8f);
                        create_one_disk(0.3f,0.8f);
                        gaming_time = 0f;
                        trail += 1;
                    }
                }
                if(round == 4)
                {
                    if(gaming_time>=1)
                    {
                        create_one_disk(-0.1f,0.5f);
                        create_one_disk(-0.1f,0.5f);
                        create_one_disk(-0.1f,0.5f);
                        gaming_time = 0f;
                        trail += 1;
                    }
                }
                if(round == 5)
                {
                    if(gaming_time>=1)
                    {
                        create_one_disk(-0.1f,-0.1f);
                        create_one_disk(-0.1f,-0.1f);
                        create_one_disk(-0.1f,-0.1f);
                        gaming_time = 0f;
                        trail += 1;
                    }
                }
            }
            else if(action_manager.get_action_num()==0)// 一个回合已经结束
            {
                if(round < 5)
                {
                    game_status = 3;// 进入回合间等待
                    waiting_time = 0;
                    gaming_time = 0;
                }
                else// 所有回合已结束
                {
                    Cursor.lockState = CursorLockMode.None;
                    game_status = 2;
                }
            }
        }

        public void free_disk(GameObject disk)
        {
            disk_factory.free_disk(disk);
        }
        void reset()
        {
            round = 1;
            trail = 1;
            game_status = 1;
            gaming_time = 0;
            waiting_time = 0;
            score_controller.clear_score();
            Cursor.lockState = CursorLockMode.Locked;
        }
        public void start()
        {
            reset();
        }
        public void restart()
        {
            reset();
        }
        public int get_game_status()
        {
            return game_status;
        }

        public void shoot(GameObject g,int shoot_status)
        {
            DiskData disk = g.GetComponent<DiskData>();
            disk.hp--;
            if(disk.hp < 1)
            {
                g.SetActive(false);
                g.transform.position = new Vector3(g.transform.position.x,-5,0);
                score_controller.record(disk.color,shoot_status);
            }
            
        }

        public int get_score()
        {
            return score_controller.get_score();
        }

        public int get_bullct()
        {
            return player.get_bullct();
        }

        int get_shoot_status()
        {
            return player.get_shoot_status();
        }

        int IUserAction.get_shoot_status()
        {
            return player.get_shoot_status();
        }
    }
}


