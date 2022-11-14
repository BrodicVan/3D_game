# 第五次3D编程作业：飞碟射击游戏

[**本次作业源代码链接：点击此处进行跳转**](https://github.com/BrodicVan/3D_game)  
[**游戏视频**](https://www.bilibili.com/video/BV1Be4y1W7mT/?vd_source=057a2b7e5be3dc8b29f8d32fd4e65aeb)
## 一.、作业要求
- 游戏内容  
-- 游戏有 n 个 round，每个 round 都包括10 次 trial；  
-- 每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同， 由该 round 的 ruler 控制；  
-- 每个 trial 的飞碟有随机性，总体难度随 round 上升；
  
- 游戏要求  
-- 使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！  
-- 尽可能使用前面 MVC 结构实现人机交互与游戏模型分离


## 二、游戏内容简述
1. 飞碟从某一区域射出，以抛物线运动轨迹移动
2. 玩家需要控制准星、走位和换弹时机在飞碟落地或飞出界外前击中飞碟
3. 不同的射击模式和飞碟类型增加的分数不尽相同：单发模式射击难度更大，加分更多  
   单发：绿/蓝/红——2/3/4  
   连发：绿/蓝/红——1/2/3
4. 共有10个回合。每回合有10次发射，每次发射会有3个飞碟射出  
   且随着回合数增加，高速度飞碟的生成概率会变大

## 三、游戏实现
**作业代码较多，此博客只挑选重要部分进行展示，[完整代码请点击此链接下载查看](https://github.com/BrodicVan/3D_game)**  
### 1. 工厂模式: DiskData.cs DiskFactory.cs
DiskData.cs：记录飞碟的基本信息
```C#

public class DiskData : MonoBehaviour
{
    public int color{set;get;}// 1—绿 2—蓝 3—红
    public float speed{set;get;}// 飞碟的加速比例
    public int hp{set;get;}// 飞碟血量，可以来调节游戏难度
    public DiskData(int c, float s, int h)
    {
        color = c;
        speed = s;
        hp = h;
    }
}
```
DiskFactory.cs：飞碟工厂，控制飞碟的生成和回收
```C#
// 单实例模板
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if(instance==null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if(instance==null)
                {
                    Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none");
                }
            }
            return instance;
        }
    }
}

// 飞碟工厂
public class DiskFactory : MonoBehaviour
{
    public GameObject disk_prefab;
    private List<DiskData> used;
    private List<DiskData> free;
    // Start is called before the first frame update
    void Start()
    {
        disk_prefab = Resources.Load<GameObject>("hw_5/Disk");
        used = new List<DiskData>();
        free = new List<DiskData>();
    }


    // 获取飞碟
    public DiskData get_disk(int color)
    {
        DiskData d = null;
        int free_size = free.Count;
        // 寻找符合空闲飞碟
        for(int i = 0; i < free_size;i++)
        {   
            if(free[i].color==color)
            {

                d = free[i];
                free.RemoveAt(i);
                break;
            }
        }
        // 若没有符合要求的空闲飞碟，则需要生成新的
        if( d == null )
        {
            d = Instantiate<GameObject>(disk_prefab).AddComponent<DiskData>();
            d.color = color;
            d.speed = color*1.0f;
            
        }
        if(color==1)
        {   
            d.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        if(color==2)
        {
            d.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        if(color==3)
        {
            d.gameObject.GetComponent<MeshRenderer>().material.color = Color.red; 
        }
        d.hp = 1;// 重置飞盘血量
        used.Add(d);
        d.gameObject.SetActive(true);    
        return d;
    }

    // 释放飞碟
    public void free_disk(GameObject disk)
    {
        int used_size = used.Count;
        for(int i = 0; i < used_size; i++)
        {
            if(disk==used[i].gameObject)
            {
                free.Add(disk.GetComponent<DiskData>());
                used.RemoveAt(i);
                break;
            }
        }
    }
}
```

### 2. 第一人称玩家: PlayControl.cs
**这部分代码由上一次作业的第一人称玩家代码修改而来**
```C#
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
        gun_sound.clip = Resources.Load<AudioClip>("hw_5/gun");
        gun_sound.pitch = 2;
        gun_sound.volume = 0.5f;

        // 换弹声音源
        reload_sound = new GameObject().AddComponent<AudioSource>();
        reload_sound.gameObject.transform.parent = this.gameObject.transform;
        reload_sound.gameObject.transform.localPosition = new Vector3(0,0.5f,0);
        reload_sound.clip = Resources.Load<AudioClip>("hw_5/reload");

        // 切换模式音源
        switch_sound = new GameObject().AddComponent<AudioSource>();
        switch_sound.gameObject.transform.parent = this.gameObject.transform;
        switch_sound.gameObject.transform.localPosition = new Vector3(0,0.5f,0);
        switch_sound.clip = Resources.Load<AudioClip>("hw_5/switch");
        
        Skybox sky = camera.gameObject.AddComponent<Skybox>();
        sky.material = Resources.Load<Material>("hw_5/sky_material");
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
            cameraAngle += 1.1f;// 枪口上抬
            bullet--;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if(Physics.Raycast(ray, out hit))
            {
                GameObject hit_object = hit.collider.gameObject;
                if(hit_object.GetComponent<DiskData>() != null)// 若命中飞碟
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
                cameraAngle += 1.5f;// 枪口上抬
                bullet--;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    GameObject hit_object = hit.collider.gameObject;
                    if(hit_object.GetComponent<DiskData>() != null)// 若命中飞碟
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
```

### 3. 飞碟飞行: DiskFly.cs
```C#
public class DiskFly : SSAction
{
    public Vector3 start;
    public float vx;// 水平速度
    public float vy;// 垂直速度
    public float dy;// 垂直加速度
    public static DiskFly GetDiskFly(Vector3 start,float vx, float vy, float dy)
    {
        DiskFly action = ScriptableObject.CreateInstance<DiskFly>();
        action.start = start;
        action.vx = vx;
        action.vy = vy;
        action.dy = dy;
        return action;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        this.game_object.transform.position = start;
        
    }

    // Update is called once per frame
    public override void Update()
    {
        if(this.game_object.transform.position.y > -5 && this.game_object.transform.position.x < 35f && this.game_object.active)
        {
            vy += Time.deltaTime*dy;
            this.game_object.transform.position += Time.deltaTime*new Vector3(vx,vy,0);
            this.game_object.transform.rotation = Quaternion.AngleAxis(Mathf.Atan(vy/vx)*180/Mathf.PI,Vector3.forward);
        }
        else// 飞碟出界或落地
        {
            this.destroy = true;// 销毁该动作
            this.callback.SSActionEvent(this);// 通知动作管理器
        }  
    }
}
```
### 4. 场景控制：RoundController.cs
```C#
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

```

### 5. 其他代码
动作分离控制：CCActionManger.cs、SSAction.cs、SSActionManager.cs  
MVC:：SSDirector.cs、UserGUI.cs  
记录分数：ScoreController.cs

## 四、游戏游玩
**[游戏游玩视频请点击该链接查看](https://www.bilibili.com/video/BV1Be4y1W7mT/?vd_source=057a2b7e5be3dc8b29f8d32fd4e65aeb)**
