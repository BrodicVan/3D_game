using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class PlayerControl : MonoBehaviour
    {
        public SceneController controller;
        CharacterController player;
        public Animator animator;
        float bodyAngle,dx,dy,dz;
        Vector3 speed;
        float speed_rate;
        // Start is called before the first frame update
        void Start()
        {
            
            player = this.gameObject.GetComponent<CharacterController>();
            animator = this.gameObject.GetComponent<Animator>();

            bodyAngle = 0f;
            dx = dy = dz =0f;
            speed = Vector3.zero;
            speed_rate = 15f;

        }
        // Update is called once per frame
        void Update()
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
            if(controller.game_status!=GameStatusType.Run) 
            {
                return;
            }
            turn();
            move();   
            action();
        }
        void action()
        {
            animator.SetBool("run",false);
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                animator.SetBool("run",true);
            }
            
        }
        void move()
        {
            dx = Input.GetAxis("Horizontal");
            dz = Input.GetAxis("Vertical");
            if(Input.GetButton("Fire3"))
            {
                dx  *= 2f;
                dz  *= 2f;
            }
            speed = new Vector3(dx,dy,dz) * Time.deltaTime * speed_rate;
            
            player.Move(Quaternion.AngleAxis(bodyAngle,Vector3.up)*speed);
        }
        void turn()
        {
            bodyAngle = (bodyAngle + Input.GetAxis("Mouse X")) % 360;
            this.gameObject.transform.rotation = Quaternion.AngleAxis(bodyAngle,Vector3.up); 
            // cameraAngle = Mathf.Clamp(cameraAngle += Input.GetAxis("Mouse Y"), min, max);
            // camera.gameObject.transform.localRotation = Quaternion.AngleAxis(cameraAngle,Vector3.left);
            
        }
    }
}

