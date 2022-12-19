using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hw_7
{
    class EnemyCollision : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("collide");
            if(collision.collider.tag=="Player")
            {
                collision.collider.gameObject.GetComponent<Animator>().SetTrigger("die");
                this.gameObject.GetComponent<Animator>().SetTrigger("shoot");
                GameEventManager.catch_player();
                
            }
        }
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

        void OnTriggerExit(Collider other)
        {
            if(other.tag=="Player")
            {
                EnemyData data = gameObject.GetComponent<EnemyData>();
                data.follow_player = false;
                data.player = null;      
                Debug.Log("lose player"); 
            }
        }
    }
    
}

