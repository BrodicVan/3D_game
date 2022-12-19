using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class Director
    {
        public IDirectorSupportor cur_controller;
        private static volatile Director instance;
        public static Director get_director()
        {
            if(instance==null)
            {
                instance = new Director();
            }
            return instance;
        }
    }
}

