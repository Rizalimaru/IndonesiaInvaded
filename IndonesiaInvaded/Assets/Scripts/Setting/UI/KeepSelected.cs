using UnityEngine;
using UnityEngine.EventSystems;

public class KeepSelected : MonoBehaviour
{
    private GameObject lastSelectedObject;

    void Update()
    {
        // Cek apakah ada objek yang sedang terpilih
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            // Jika objek terpilih berbeda dengan objek terpilih sebelumnya, update lastSelectedObject
            if (EventSystem.current.currentSelectedGameObject != lastSelectedObject)
            {
                lastSelectedObject = EventSystem.current.currentSelectedGameObject;
            }
        }
        else
        {
            // Jika tidak ada objek terpilih, pilih kembali objek terakhir yang terpilih
            if (lastSelectedObject != null)
            {
                EventSystem.current.SetSelectedGameObject(lastSelectedObject);
            }
        }
    }
}
