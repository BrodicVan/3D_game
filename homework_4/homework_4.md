# 第三次3D编程作业

[**本次作业源代码链接：点击此处进行跳转**](https://github.com/BrodicVan/3D_game)
## 一. 基本操作演练
- 下载Fantasy Skybox Free,构建自己的游戏场景  
  场景总览图：
  ![](%E5%9C%BA%E6%99%AF%E6%80%BB%E8%A7%88.jpg)
  游玩截图：
  ![](%E6%B8%B8%E7%8E%A9%E5%9C%BA%E6%99%AF%E4%B8%80.jpg)  
  ![](%E6%B8%B8%E7%8E%A9%E5%9C%BA%E6%99%AF%E4%BA%8C.jpg)

- 写一个简单的总结，总结游戏对象的使用
  - 普通3D物体  
    普通的3D物体一般用来表示会运动的游戏物体，比如游戏玩家、敌人和机关等，一般都需要设置碰撞体积，以满足某些物理效果
  - 摄像机  
    摄像机可以作为玩家观察游戏的视角，也可以利用多摄像头实现小地图、望远镜、无人机观察的功能，同时可以收录声音
  - 光源  
    光源提供光照，为游戏场景可视化提供基本条件，也可以是各种游戏物体发光，使物体更加真实，如：手电筒、火把等
  - 地形
    地形一般表示不可改变的游戏物体，不需要运动。
  - 声源
    附加在某些物体上使其发声，使游戏更加真实

## 二. 编程实践：《大主宰》关卡原型设计：
[点击该链接查看源代码](https://github.com/BrodicVan/3D_game)

阅读天蚕土豆小说《大主宰》第975~981章，这段讲述了男主离开九幽族到神兽之原的途中在太空中穿梭的过程。  

小说情景简述：  
主角团需要在空间中的陨石间不断穿越以到达目的地神兽之原，在途中还要尽可能收集血泥块提升自身实力

### 1. 游戏世界对象和元素  
- 主角牧尘  
  主角牧尘作为小说主角，玩家需要操作其在陨石上跳跃以到达目的地，同时收集血泥块提升实力，帮助玩家通过关卡较难的部分  
  简单建一下模型(技术有限，请见谅)：
  ![](%E7%89%A7%E5%B0%98%E5%BB%BA%E6%A8%A1.jpg)

- 陨石：用移动的褐色长方体表示陨石，陨石的大小各不相同，且在关卡后半部分只会以一定概率生成，两个陨石之间的间距会变大，从而可以提高游戏难度  

- 血泥块：用扁平的红色圆柱体表示血泥块，需要附着在陨石上，可以被玩家收集  
- 神兽之原、起始陆地、中转陆地：用较大的固定长方体表示
       
### 2. 基本玩法与挑战
基本玩法：玩家需要在各陨石上跳跃移动，并最终到达目的地。在游戏过程中，玩家可以收集血泥块以解锁技能(二段跳和冲刺)，以帮助通过关卡的较难部分。  

挑战：  
(1) 玩家不会附着在移动的陨石上，且陨石只会存在一定时间，移动到一定距离后就会消失
(2) 到了关卡的后半部分，陨石的生成概率会变小，且移动速度会更快，有的时候会出现玩家可能无法跨越的距离，此时玩家就需要进行权衡，是继续前进还是回到中转点等待机会
(3) 玩家掉落虚空会传送回出生点，且先前收集的血泥块与获得的技能都清空

### 3. 管理游戏物体的运动
(1) 陨石：  
   陨石一般由起始地点生成，以恒定速度向目的地前进，到达目的地后会被释放。
   控制其运动的脚本如下：
   ```C#
   // StoneAction.cs
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    namespace hw_4
    {
        public class StoneAction : MonoBehaviour
        {
            Vector3 destination;
            Vector3 speed;
            // Update is called once per frame
            void Update()
            {
                this.gameObject.transform.position += speed*Time.deltaTime*2;// 向目的地移动
                if(Vector3.Distance(this.gameObject.transform.position,destination)<0.1f)// 到达目的地后释放
                    Destroy(this.gameObject);
            }
            public void set_destination(Vector3 des)
            {
                destination = des;
            }
            public void set_speed(Vector3 s)
            {
                speed = s;
            }
        }
    }
   ```

(2) 玩家：  
   玩家以第一人称进行游戏，可以解锁二段跳和冲刺。
   ```C#
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    namespace hw_4
    {
        public class PlayControl : MonoBehaviour
        {
            float cameraAngle;// 摄像机旋转的角度
            float bodyAngle;// 身体旋转的角度
            float min,max;
            private CharacterController player;// 玩家控制器
            private Vector3 speed;
            private float dy;// 向下的速度
            private Camera camera;
            private int jump_times;// 跳跃次数，辅助实现二段跳
            public int blood_num;// 收集的血泥块数量

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
                camera.transform.parent = this.gameObject.transform;
                camera.transform.localPosition = new Vector3(0,1,0);
                Skybox sky = camera.gameObject.AddComponent<Skybox>();
                sky.material = Resources.Load<Material>("hw_4/sky_material");
                // 固定并隐藏鼠标
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = true;
                jump_times = 0;
            }

            // Update is called once per frame
            void Update()
            {
                turn();
                jump();
                move();
                reset();
            }
            void jump()
            {
                if(player.isGrounded)// 玩家落地重置跳跃次数
                    jump_times = 0;
                if(Input.GetButtonDown("Jump"))
                {
                    if(jump_times == 0)// 一段跳
                    {
                        dy = 3;
                        jump_times += 1;
                    }
                    else if(jump_times == 1 && blood_num >= 3)// 二段跳
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
                if(Input.GetButton("Fire3") && blood_num >= 10)// 冲刺
                {
                    dx  *= 3;
                    dz  *= 3;
                }
                speed = new Vector3(dx,dy,dz) * Time.deltaTime;
                player.Move(Quaternion.AngleAxis(bodyAngle,Vector3.up)*speed);// 此处需要根据当前的旋转度调整移动的方向
            }
            void turn()
            {
                bodyAngle = (bodyAngle + Input.GetAxis("Mouse X")) % 360;
                this.gameObject.transform.rotation = Quaternion.AngleAxis(bodyAngle,Vector3.up); 
                cameraAngle = Mathf.Clamp(cameraAngle += Input.GetAxis("Mouse Y"), min, max);
                camera.gameObject.transform.localRotation = Quaternion.AngleAxis(cameraAngle,Vector3.left);
            }

            // 角色掉下虚空游戏重置
            void reset()
            {
                if(this.gameObject.transform.position.y < -10f)
                {
                    this.gameObject.transform.position = new Vector3(30f,5f,8.5f);
                    blood_num = 0;
                }
            }
        }
    }
   ```

4. 演示视频：[请点击该链接进行查看](https://www.bilibili.com/video/BV1ev4y1D7JQ/?vd_source=057a2b7e5be3dc8b29f8d32fd4e65aeb)
   
   

