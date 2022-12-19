using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class FindCollision : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if(other.tag=="Player")
            {
                
                EnemyData data = gameObject.GetComponent<EnemyData>();
                data.follow_player = true;
                data.player = other.gameObject;     
                Debug.Log("find player"); 
            }
        }
    }
}

