public interface ISceneController
{
    void load_resources();
    void update_game_status();
}

public class SSDirector : System.Object
{

    private static SSDirector _instance;
    private ISceneController cur_controller;

    public static SSDirector get_director()
    {
        if(_instance ==null) _instance = new SSDirector();
        return _instance;
    }

    public void set_controller(ISceneController controller)
    {
        cur_controller = controller;
    }

    public ISceneController get_controller()
    {
        return cur_controller;
    }
}
