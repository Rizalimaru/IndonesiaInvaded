using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    public Transform objectA; // Objek yang akan bergerak
    public Transform objectB; // Objek yang akan dituju

    public float speed = 5f; // Kecepatan pergerakan objek A

    private void Update()
    {
        // Pastikan objectA dan objectB tidak null
        if (objectA != null && objectB != null)
        {
            // Hitung arah menuju objectB
            Vector3 direction = (objectB.position - objectA.position).normalized;

            // Hitung pergerakan objek A
            Vector3 movement = direction * speed * Time.deltaTime;

            // Pindahkan objek A menuju objek B
            objectA.Translate(movement);
        }
        else
        {
            Debug.LogWarning("ObjectA or ObjectB is not assigned!");
        }
    }
}
