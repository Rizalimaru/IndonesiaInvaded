using UnityEngine;

public class TiltedOrbit : MonoBehaviour
{
    private GameObject target; // Target yang akan diorbitkan
    public float orbitSpeed = 10.0f; // Kecepatan orbit
    public float orbitDistance = 5.0f; // Jarak dari target

    private Vector3 offset;

    void Start()
    {   
        target = GameObject.Find("CameraLookPoint"); // Mencari objek Player
        // Menghitung offset awal berdasarkan jarak orbit
        offset = new Vector3(0, 0, -orbitDistance);
    }

    void Update()
    {
        // Rotasi offset seputar target berdasarkan kecepatan orbit dan waktu
        offset = Quaternion.AngleAxis(orbitSpeed * Time.deltaTime, Vector3.up) * offset;

        // Mengatur posisi objek ini berdasarkan posisi target dan offset
        transform.position = target.transform.position + offset;

        // Mengatur arah objek ini menghadap ke target
        transform.LookAt(target.transform);
    }
}
