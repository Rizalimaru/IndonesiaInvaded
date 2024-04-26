using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_ScrollBarMenu : MonoBehaviour
{
    public Scrollbar scrollBar;
    float[] pos;
    float scroll_pos;
    float distance; // pindahkan deklarasi variabel distance ke sini

    void Start()
    {
        pos = new float[transform.childCount];
        distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        // Inisialisasi scroll_pos ke pos[0] agar scroll bar berada di indeks paling awal
        scroll_pos = pos[0];
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollBar.value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollBar.value = Mathf.Lerp(scrollBar.value, pos[i], 0.1f);
                }
            }
        }

    }
}
