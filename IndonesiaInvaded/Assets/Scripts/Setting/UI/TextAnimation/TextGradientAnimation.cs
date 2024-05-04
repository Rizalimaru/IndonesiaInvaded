using UnityEngine;
using TMPro;

public class TextGradientAnimation : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public Gradient gradient;
    public float animationSpeed = 1f;

    private void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
        }

        // Memastikan teks memiliki Rich Text diaktifkan untuk mendukung gradien.
        textMeshPro.enableVertexGradient = true;
    }

    private void Update()
    {
        // Menghitung offset berdasarkan waktu untuk animasi gradien.
        float offset = Time.time * animationSpeed;
        
        // Menerapkan gradien yang dihasilkan dari waktu ke teks.
        textMeshPro.colorGradient = EvaluateGradient(offset);
    }

    private VertexGradient EvaluateGradient(float offset)
    {
        // Evaluasi gradien pada waktu tertentu dan konversi ke tipe VertexGradient.
        Color color = gradient.Evaluate(Mathf.Repeat(offset, 1f));
        VertexGradient vertexGradient = new VertexGradient(color);

        // Anda juga bisa menyesuaikan nilai-nilai lain seperti warna tepi, dll. sesuai kebutuhan.

        return vertexGradient;
    }
}
