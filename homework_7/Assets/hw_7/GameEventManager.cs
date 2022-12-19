using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hw_7
{
    class GameEventManager : System.Object
    {
        private static GameEventManager _instance;

        public delegate void score_change();
        public static event score_change score_change_should_do; 

        public delegate void game_lose();
        public static event game_lose game_lose_should_do; 

        public delegate void game_win();
        public static event game_win game_win_should_do; 

        public delegate void game_start();
        public static event game_start game_start_should_do; 

        public static void player_escape()
        {
            if( score_change_should_do!=null )
            {
                score_change_should_do();
            }
        }

        public static void catch_player()
        {
            if( game_lose_should_do!=null )
            {
                game_lose_should_do();
            }
        }

        public static void reach_final()
        {
            if(game_win_should_do!=null)
            {
                game_win_should_do();
            }
        }

        public static void start_game()
        {
            if(game_start_should_do!=null)
            {
                game_start_should_do();
            }
        }
        
        public static GameEventManager get_instance()
        {
            if(_instance==null)
            {
                _instance = new GameEventManager();
            }
            return _instance;
        }

        
    }    
}
