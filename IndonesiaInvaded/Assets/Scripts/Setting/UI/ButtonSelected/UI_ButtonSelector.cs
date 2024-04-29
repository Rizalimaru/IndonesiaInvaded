using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    public GameObject objectSettings; // Objek yang ingin dimonitor statusnya
    public Button button; // Button yang akan diubah status selected/deselected

    public Animator animator; // Animator yang akan diubah statusnya

    void Update()
    {
        if (objectSettings.activeSelf)
        {
            button.Select(); // Memilih button jika objek audio aktif
            // Mengdisable animasi apapu seperti hover, pressed, dll jika button sedang dipilih
            button.transition = Selectable.Transition.None;
            // mengdisable onclick jika button sedang dipilih
            animator.SetTrigger("Selected"); // Mengubah status animator jika objek audio aktif
        }
        else
        {
            button.OnDeselect(null); // Menghilangkan seleksi jika objek audio tidak aktif
            animator.SetTrigger("Deselected"); // Mengubah status animator jika objek audio tidak aktif#
            // Mengenable animasi apapun seperti hover, pressed, dll jika button tidak sedang dipilih
            button.transition = Selectable.Transition.ColorTint;
        }
    }
}
