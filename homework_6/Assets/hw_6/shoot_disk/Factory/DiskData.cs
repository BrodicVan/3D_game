using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_6
{
    public class DiskData : MonoBehaviour
    {
        public int color{set;get;}// 1—绿 2—蓝 3—红
        public float speed{set;get;}// 1/2/3
        public int hp{set;get;}
        public DiskData(int c, float s, int h)
        {
            color = c;
            speed = s;
            hp = h;
        }
    }
}

