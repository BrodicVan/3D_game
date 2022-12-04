# 第五次3D编程作业：飞碟射击游戏

[**本次作业源代码链接：点击此处进行跳转**](https://github.com/BrodicVan/3D_game)  
[**游戏视频**](https://www.bilibili.com/video/BV1NG4y1V7sz/?vd_source=057a2b7e5be3dc8b29f8d32fd4e65aeb)
## 一.、作业要求
1. 按adapter模式设计图修改飞碟游戏
2. 使它同时支持物理运动与运动学（变换）运动


## 二、游戏内容简述
1. 飞碟从某一区域射出，以抛物线运动轨迹移动
2. 玩家需要控制准星、走位和换弹时机在飞碟落地或飞出界外前击中飞碟
3. 不同的射击模式和飞碟类型增加的分数不尽相同：单发模式射击难度更大，加分更多  
   单发：绿/蓝/红——2/3/4  
   连发：绿/蓝/红——1/2/3
4. 共有10个回合。每回合有10次发射，每次发射会有3个飞碟射出  
   且随着回合数增加，高速度飞碟的生成概率会变大

## 三、游戏实现
**作业代码与作业五大体类似，此博客只挑选不同部分进行展示，[完整代码请点击此链接下载查看](https://github.com/BrodicVan/3D_game)**  
### 1.动作管理 
SSAction.cs
```C#
public class SSAction : ScriptableObject
{
    public bool enable = true;
    public bool destroy = false;

    public GameObject game_object{get;set;}
    // public Transform transform{get;set;}
    public ISSActionCallback callback{get;set;}

    protected SSAction() {}
    // Start is called before the first frame update
    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }

    // 在此处添加FixedUpdate的管理
    public virtual void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }

}
```

PhyActionManager.cs
```C#
public class PhyActionManager : PhySSActionManager, ISSActionCallback
{
    public RoundController controller;
    // Start is called before the first frame update
    protected new void Start()
    {
        controller = SSDirector.get_instance().current_controller as RoundController;
    }

    // Update is called once per frame
    protected new void Update()
    {
        if(controller.get_game_status()==1)// 游戏运行中才更新物体状态
            base.Update();
    }
    protected new void FixedUpdate()
    {
        if(controller.get_game_status()==1)// 游戏运行中才更新物体位置
            base.FixedUpdate();
    }
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed,
                int intParam = 0, string strParam = null, Object objectParam = null )
    {
        if(source is PhyDiskFly)
        {
            controller.free_disk(source.game_object);
        }
    }
}
```

PhySSActionManager.cs
```C#
public class PhySSActionManager : MonoBehaviour
{
    // ...

    // Update is called once per frame
    protected void Update()
    {
        // ...
    }
    protected void FixedUpdate()
    {
        foreach(KeyValuePair<int,SSAction> kv in actions)
        {
            SSAction ac = kv.Value; 
            if(ac.enable)// 若可行
            {
                ac.FixedUpdate();
            }
        }
    }
    public int get_action_num()
    {
        return actions.Count;
    } 
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.game_object = gameobject;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
        // 开启刚体，并关闭运动学控制
        Rigidbody rigidbody = gameobject.GetComponent<Rigidbody>();
        if(rigidbody==null)
            rigidbody = gameobject.AddComponent<Rigidbody>();
        rigidbody.isKinematic=false;
    }

    protected void Start()
    {}

}
```


### 2. 物理控制的飞碟飞行: PhyDiskFly.cs、

```C#
public class PhyDiskFly : SSAction
{
    public Vector3 start;
    public float vx_0;// 水平初速度
    public float vy_0;// 垂直初速度
    public float dy;// 垂直加速度
    public static PhyDiskFly GetPhyDiskFly(Vector3 start,float vx, float vy, float dy)
    {
        // ...
    }
    // Start is called before the first frame update
    public override void Start()
    {
        // ...
    }

    // Update函数用来判断是否释放该飞碟
    public override void Update()
    {
        if(this.game_object.transform.position.y > -5 && this.game_object.transform.position.x < 40f && this.game_object.activeSelf)
        {
            // 此处将运动状态变化移出Update()函数
            // vy += Time.deltaTime*dy;
            // this.game_object.transform.position += Time.deltaTime*new Vector3(vx,vy,0);
            // this.game_object.transform.rotation = Quaternion.AngleAxis(Mathf.Atan(vy/vx)*180/Mathf.PI,Vector3.forward);
        }
        else// 飞碟出界或落地
        {
            this.destroy = true;// 销毁该动作
            this.callback.SSActionEvent(this);// 通知动作管理器
        }  
    }

    // 将FixedUpdate具体实现
    public override void FixedUpdate()
    {
        Rigidbody rigidbody = this.game_object.GetComponent<Rigidbody>();
        rigidbody.AddForce(0,dy,0,ForceMode.Acceleration);// 添加向下的作用力
        rigidbody.rotation = Quaternion.AngleAxis(Mathf.Atan(rigidbody.velocity.y/rigidbody.velocity.x)*180/Mathf.PI,Vector3.forward);// 调整飞碟的旋转角度
    }
}
```

### 3.Adapter模式：此处使用的对象适配器
```C#
public interface IActionManger
{
    void play_disk(GameObject disk,bool if_phy,Vector3 start,float vx,float vy,float dy);
    int get_action_num();
}

public class ActionAdapter : System.Object,IActionManger
{
    RoundController controller;
    // 两种对象动作管理器对象
    PhyActionManager phy_manager
    CCActionManger cc_manager;

    public ActionAdapter()
    {
        controller = SSDirector.get_instance().current_controller as RoundController;            
        phy_manager = new GameObject().AddComponent<PhyActionManager>();
        phy_manager.controller= controller;
        cc_manager = phy_manager.gameObject.AddComponent<CCActionManger>();
        cc_manager.controller = controller;
    }

    public int get_action_num()
    {
        return phy_manager.get_action_num() + cc_manager.get_action_num();
    }

    // 根据是否选择物理学运功使用对应的动作管理器管理动作
    public void play_disk(GameObject disk,bool if_phy, Vector3 start, float vx, float vy, float dy)
    {
        if(if_phy)
        {
            phy_manager.RunAction(disk,PhyDiskFly.GetPhyDiskFly(start,vx,vy,dy),phy_manager);
        }
        else
        {
            // Debug.Log("cc");
            cc_manager.RunAction(disk,DiskFly.GetDiskFly(start,vx,vy,dy),cc_manager);
        }
    }
}
```

### 4. 场景控制：RoundController.cs
```C#
public class RoundController : MonoBehaviour,IUserAction, IPlayer, ISceneController
    {
        //...省略一系列成员变量

        bool if_phy;// 是否为物理学控制飞碟的运动
        // Start is called before the first frame update
        void Start()
        {
            // ...
            if_phy = true;//此处将if_phy设为true，是为了防止bug，在实际运行中玩家会通过UI选择调整if_phy
           
        }

        // Update is called once per frame
        void Update()
        {
            //...
        }

        
        void create_one_disk(float small,float mid,float z_bia)// 此处设置多一个在z轴上的偏移，防止飞碟加入刚体后的相互碰撞
        {
            // ...
            acm.play_disk(disk.gameObject,if_phy, start,vx*disk.speed,vy,dy);// 动作管理器根据是否物理学状态生成飞碟
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
                        // 每一轮的三个飞碟的z轴设置合适的偏移使其不要碰撞，后同
                        create_one_disk(1,1,-4);
                        create_one_disk(1,1,0);
                        create_one_disk(1,1,4);
                        gaming_time = 0f;
                        trail += 1;
                    }
                }
                if(round == 2)
                {
                    if(gaming_time>=1)
                    {
                        // ... 
                    }
                }
                if(round == 3)
                {
                    if(gaming_time>=1)
                    {
                        // ...
                    }
                }
                if(round == 4)
                {
                    if(gaming_time>=1)
                    {
                        // ...
                    }
                }
                if(round == 5)
                {
                    if(gaming_time>=1)
                    {
                        // ...
                    }
                }
            }
            else if(acm.get_action_num()==0)// 一个回合已经结束
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

        // 以下函数根据if_phy重置游戏状态
        void reset(bool if_phy)
        {
            round = 1;
            trail = 1;
            game_status = 1;
            gaming_time = 0;
            waiting_time = 0;
            score_controller.clear_score();
            Cursor.lockState = CursorLockMode.Locked;
            this.if_phy = if_phy;
        }
        public void start(bool if_phy)
        {
            reset(if_phy);
        }
        public void restart(bool if_phy)
        {
            reset(if_phy);
        }
        public int get_game_status()
        {
            return game_status;
        }

        // ...省略一系列函数...
    }

```

## 四、游戏游玩
**[游戏游玩视频请点击该链接查看](https://www.bilibili.com/video/BV1NG4y1V7sz/?vd_source=057a2b7e5be3dc8b29f8d32fd4e65aeb)**
