using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class SceneController : MonoBehaviour, IGUISupportor,IDirectorSupportor
    {
        public int play_area;

        ScoreController score_controller;
        EnemyActionManager enemy_action_manager;
        MyGUI gui;
        public GameStatusType game_status;
        EnemyFactory factory;
        PlayerControl player;
        List<GameObject> enemies;
        

        // Start is called before the first frame update
        void Start()
        {
            // Physics.autoSyncTransforms = true;
            Director.get_director().cur_controller = this;
            GameObject container = new GameObject();
            score_controller = new ScoreController();
            enemy_action_manager = container.AddComponent<EnemyActionManager>();
            gui = container.AddComponent<MyGUI>();
            gui.set_support(this);
            factory =  container.AddComponent<EnemyFactory>();
            load_resource();
            game_status = GameStatusType.Ready;
        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log(play_area);
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyData>().player_area = play_area;
                enemy.GetComponent<EnemyData>().player = player.gameObject;
            }

        }

        void OnEnable()
        {
            GameEventManager.score_change_should_do += add_score;
            GameEventManager.game_lose_should_do += game_lose;
            GameEventManager.game_win_should_do += game_win;
        }
        
        void game_lose()
        {
            play_area = 0;
            game_status = GameStatusType.Lose;
            stop_enemy();
            Cursor.lockState = CursorLockMode.None;
        }

        void game_win()
        {
            game_status = GameStatusType.Win;
            player.gameObject.GetComponent<Animator>().SetBool("run",false);
            stop_enemy();
            Cursor.lockState = CursorLockMode.None;
        }

        void game_start()
        {
            game_status = GameStatusType.Run;
            run_enemy();
            score_controller.score = 0;
            Destroy(player.gameObject);
            
            player = Instantiate(Resources.Load<GameObject>("hw_7/player"),new Vector3(0f,0f,-35f),Quaternion.Euler(0,0,0)).
                        GetComponent<PlayerControl>();
            player.controller = this;

            // player.gameObject.transform.position = new Vector3(0f,0f,-35f);
            Cursor.lockState = CursorLockMode.Locked;
            // player.GetComponent<Animator>().SetTrigger("revive");
        }

        void stop_enemy()
        {
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponent<Animator>().SetBool("run",false);
            }
            enemy_action_manager.destroy_all();
        }

        
        void run_enemy()
        {
            foreach(GameObject enemy in enemies)
            {
                enemy_action_manager.run_enemy(enemy);
                enemy.GetComponent<Animator>().SetBool("run",true);
            }
        }
        void add_score()
        {
            score_controller.score += 1;
        }
        void load_resource()
        {
            Instantiate<GameObject>(Resources.Load<GameObject>("hw_7/map"));
            player = Instantiate(Resources.Load<GameObject>("hw_7/player"),new Vector3(0f,0f,-35f),Quaternion.Euler(0,0,0)).
                        GetComponent<PlayerControl>();

            player.controller = this;
            enemies = factory.get_enemies();
            
            Instantiate<Canvas>(Resources.Load<Canvas>("hw_7/minimap_canvas"));
        }

        public GameStatusType get_game_status()
        {
            return game_status;
        }

        public void start_game()
        {
            game_start();
        }

        public int get_score()
        {
            return score_controller.score;
        }
    }
}

