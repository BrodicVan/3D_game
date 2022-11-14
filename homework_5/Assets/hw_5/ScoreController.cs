using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_5
{
    public class ScoreController : System.Object
    {
        int score;
        public ScoreController()
        {
            score = 0;
        }

        public void Reset()
        {
            score = 0;
        }

        public int get_score()
        {
            return score;
        }

        public void record(int color,int shoot_status)
        {
            score += color+shoot_status;
        }

        public void clear_score()
        {
            score = 0;
        }

    }
}
