using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace hw_6
{
    // 场景控制器需要实现的接口
    public interface IPlayer
    {
        void shoot(GameObject g,int shoot_status);
        int get_game_status();
    }
    public class PlayControl : MonoBehaviour
    {
        float cameraAngle;  //摄像机（头）旋转的角度
        float bodyAngle;   //身体旋转的角度
        float min,max;
        private CharacterController player;
        private Vector3 speed;
        private float dy;
        private Camera camera;
        private int jump_times;
        private IPlayer controller;
        private int bullet;// 当前子弹数
        private bool Ring;// 是否在装弹
        private int shoot_status;//0—连发  1—点射
        private AudioSource gun_sound;
        private AudioSource reload_sound;
        private AudioSource switch_sound;
        private float shoot_time;// 控制连发的射击速度
        private float reload_time;// 控制装弹速度
        // Start is called before the first frame update
        void Start()
        {
            player = this.gameObject.AddComponent<CharacterController>();
            player.skinWidth = 0.01f;
            speed = Vector3.zero;
            cameraAngle = 0;
            bodyAngle = 0;
            min = -55;
            max = 60;
            camera = new GameObject().AddComponent<Camera>();
            camera.gameObject.transform.parent = this.gameObject.transform;
            camera.gameObject.transform.localPosition = new Vector3(0,1,0);
            camera.gameObject.AddComponent<AudioListener>();

            // 枪声音源
            gun_sound = new GameObject().AddComponent<AudioSource>();
            gun_sound.gameObject.transform.parent = this.gameObject.transform;
            gun_sound.gameObject.transform.localPosition = new Vector3(0,0.5f,0);
            gun_sound.clip = Resources.Load<AudioClip>("hw_6/gun");
            gun_sound.pitch = 2;
            gun_sound.volume = 0.5f;

            // 换弹声音源
            reload_sound = new GameObject().AddComponent<AudioSource>();
            reload_sound.gameObject.transform.parent = this.gameObject.transform;
            reload_sound.gameObject.transform.localPosition = new Vector3(0,0.5f,0);
            reload_sound.clip = Resources.Load<AudioClip>("hw_6/reload");

            // 切换模式音源
            switch_sound = new GameObject().AddComponent<AudioSource>();
            switch_sound.gameObject.transform.parent = this.gameObject.transform;
            switch_sound.gameObject.transform.localPosition = new Vector3(0,0.5f,0);
            switch_sound.clip = Resources.Load<AudioClip>("hw_6/switch");
            
            Skybox sky = camera.gameObject.AddComponent<Skybox>();
            sky.material = Resources.Load<Material>("hw_6/sky_material");
            jump_times = 0;
            Physics.autoSyncTransforms = true;
            bullet = 45;
            reload_time = 0;
            shoot_status = 0;
            shoot_time = 0;
        }

        // Update is called once per frame
        void Update()
        {
            // 若游戏状态为未开始或已结束，则无法操控第一人称玩家
            int game_status = controller.get_game_status();
            if(game_status==0||game_status==2) return;
            reload();
            switch_status();
            shoot();
            turn();
            jump();
            move();    
            reset();
        }
        void jump()
        {
            if(player.isGrounded)
                jump_times = 0;
            if(Input.GetButtonDown("Jump"))
            {
                
                if(jump_times == 0)
                {
                    dy = 3;
                    jump_times += 1;
                }
                else if(jump_times == 1)
                {
                    dy = 3;
                    jump_times += 1;
                }
            }
            dy -= Time.deltaTime*9.8f;
        }
        void move()
        {
            float dx = Input.GetAxis("Horizontal")*2;
            float dz = Input.GetAxis("Vertical")*2;
            if(Input.GetButton("Fire3"))
            {
                dx  *= 3;
                dz  *= 3;
            }
            speed = new Vector3(dx,dy,dz) * Time.deltaTime;
            
            player.Move(Quaternion.AngleAxis(bodyAngle,Vector3.up)*speed);
        }
        void turn()
        {
            bodyAngle = (bodyAngle + Input.GetAxis("Mouse X")) % 360;
            this.gameObject.transform.rotation = Quaternion.AngleAxis(bodyAngle,Vector3.up); 
            cameraAngle = Mathf.Clamp(cameraAngle += Input.GetAxis("Mouse Y"), min, max);
            camera.gameObject.transform.localRotation = Quaternion.AngleAxis(cameraAngle,Vector3.left);
            
        }
        void shoot()
        {   
            if(Ring) return;
            if(bullet==0) 
            {
                reload_sound.Play();
                Ring = true;
                reload_time = 0f;
                return;
            }
            //单发模式
            if( shoot_status==1 && Input.GetMouseButtonDown(0))
            {
                gun_sound.Play();
                cameraAngle += 1.1f;
                bullet--;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    GameObject hit_object = hit.collider.gameObject;
                    if(hit_object.GetComponent<DiskData>() != null)
                    {
                        controller.shoot(hit_object,shoot_status);
                    }
                }
            }
            // 连发模式
            if( shoot_status==0 && Input.GetMouseButton(0))
            {
                shoot_time += Time.deltaTime;
                if(shoot_time > 0.1f)
                {
                    shoot_time=0;
                    gun_sound.Play();
                    cameraAngle += 1.5f;
                    bullet--;
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit))
                    {
                        GameObject hit_object = hit.collider.gameObject;
                        if(hit_object.GetComponent<DiskData>() != null)
                        {
                            controller.shoot(hit_object,shoot_status);
                        }
                    }
                }
            }
            
        }

        // 换弹
        void reload()
        {
            if(Ring)
            {
                reload_time += Time.deltaTime;
                if(reload_time>1) 
                {
                    Ring = false;
                    bullet = 45;
                }
            }
            if(Input.GetKeyUp(KeyCode.R))
            {
                reload_sound.Play();
                Ring = true;
                reload_time = 0;
            }
        }
        // 切换射击模式
        void switch_status()
        {
            if(Ring) return;
            if(Input.GetKeyUp(KeyCode.E))
            {
                switch_sound.Play();
                shoot_status = (shoot_status+1)%2;
            }
        }
        public int get_bullct()
        {
            return bullet;
        }
        public int get_shoot_status()
        {
            return shoot_status;
        }
        public void reset()
        {
            if(gameObject.transform.position.y<-5 || gameObject.transform.position.z > -25)
            {
                gameObject.transform.position = new Vector3(0,1,-30);
            }
        }

        public void set_controller(IPlayer c)
        {
            controller = c;
        }

    }
}