using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class EnemyFactory : MonoBehaviour
    {
        GameObject enemy_prefab;
        List<int> z_bias = new List<int>(new int[]{50,150,250,350});
        
        List<GameObject> used = new List<GameObject>();
        List<GameObject> free = new List<GameObject>();

        public List<GameObject> get_enemies()
        {
            List<GameObject> enemies = new List<GameObject>();
            for(int i = 0; i < 4;i++)
            {
                GameObject enemy = get_enemy(z_bias[i]);
                EnemyData data =  enemy.GetComponent<EnemyData>();
                data.enemy_area = i+2;
                enemies.Add(enemy);
            }
            return enemies;
        }

        void free_enemy(GameObject enemy)
        {
            used.Remove(enemy);
            free.Add(enemy);
        }

        GameObject get_enemy(int z_bia)
        {
            GameObject enemy;
            if(enemy_prefab==null)
            {
                enemy_prefab = Resources.Load<GameObject>("hw_7/enemy");
            }
            if(free.Count>0)
            {
                enemy = free[0];
                free.Remove(enemy);
                enemy.transform.position = new Vector3(0,0,z_bia);
            }
            else
            {
                enemy = Instantiate<GameObject>(enemy_prefab,new Vector3(0,0,z_bia),Quaternion.Euler(0,0,0));
                used.Add(enemy);
            }
            return enemy;
        }

        // Start is called before the first frame update
        void Start()
        {
            enemy_prefab = Resources.Load<GameObject>("hw_7/enemy");//
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        
    }
}

