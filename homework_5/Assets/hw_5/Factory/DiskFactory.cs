using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_5
{
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
}


