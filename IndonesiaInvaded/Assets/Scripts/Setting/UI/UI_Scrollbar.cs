using UnityEngine;
using UnityEngine.UI;

public class UI_Scrollbar : MonoBehaviour
{
    public Scrollbar scrollbar;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            scrollbar.value += scroll * 1.5f;
        }
    }
}
