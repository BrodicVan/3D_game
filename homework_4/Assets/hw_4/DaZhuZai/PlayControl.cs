using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace hw_4
{
    public partial interface ISceneController
    {
        void reset();
    }
    public class PlayControl : MonoBehaviour
    {
        float cameraAngle;  //摄像机（头）旋转的角度
        float bodyAngle;   //身体旋转的角度
        float min,max;
        private CharacterController player;
        private Vector3 speed;
        private float dy;
        private float pp;
        private Camera camera;
        private int jump_times;
        private int blood_num;
        private ISceneController controller;
        // Start is called before the first frame update
        void Start()
        {
            player = this.gameObject.AddComponent<CharacterController>();
            player.skinWidth = 0.01f;
            speed = Vector3.zero;
            cameraAngle = 0;
            bodyAngle = 0;
            min = -55;
            max = 30;
            camera = new GameObject().AddComponent<Camera>();
            camera.gameObject.transform.parent = this.gameObject.transform;
            camera.gameObject.transform.localPosition = new Vector3(0,1,0);
            Skybox sky = camera.gameObject.AddComponent<Skybox>();
            sky.material = Resources.Load<Material>("hw_4/sky_material");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
            jump_times = 0;
            blood_num = 0;
            pp = 200f;
            Physics.autoSyncTransforms = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(this.gameObject.transform.position.y < -10f)
                controller.reset();
            turn();
            jump();
            move();    
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
                else if(jump_times == 1 && blood_num >= 3 && pp >=10)
                {
                    dy = 3;
                    jump_times += 1;
                    pp -= 10;
                }
            }
            dy -= Time.deltaTime*9.8f;
        }
        void move()
        {
            float dx = Input.GetAxis("Horizontal")*2;
            float dz = Input.GetAxis("Vertical")*2;
            if(Input.GetButton("Fire3") && blood_num >= 10 && pp > 0)
            {
                pp -= Time.deltaTime*10;
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
            Debug.LogFormat("here:{0}",camera==null);
            camera.gameObject.transform.localRotation = Quaternion.AngleAxis(cameraAngle,Vector3.left);
            
        }
        public void reset()
        {
            this.gameObject.transform.position = new Vector3(30f,5f,8.5f);
            blood_num = 0;
            pp = 200f;
        }
        public int get_blood_num()
        {
            return blood_num;
        }

        public int get_pp()
        {
            return pp>0?(int)pp:0;
        }
        public void set_controller(ISceneController c)
        {
            controller = c;
        }
        public void add_blood()
        {
            blood_num += 1;
        }
    }
}