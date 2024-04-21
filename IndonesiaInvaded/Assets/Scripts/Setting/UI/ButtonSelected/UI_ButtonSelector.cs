using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    public GameObject objectSettings; // Objek yang ingin dimonitor statusnya
    public Button button; // Button yang akan diubah status selected/deselected

    void Update()
    {
        if (objectSettings.activeSelf)
        {
            button.Select(); // Memilih button jika objek audio aktif
        }
        else
        {
            button.OnDeselect(null); // Menghilangkan seleksi jika objek audio tidak aktif
        }
    }
}
