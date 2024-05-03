using UnityEngine;

public enum Type
{
    Main_Menu,
    Pause_Menu
}

[CreateAssetMenu(fileName = "NewMenu", menuName = "Scene Data/Menu")]
public class Menus : GameScene
{
    //Settings specific to menu only
    [Header("Menu specific")]
    public Type type;
}
