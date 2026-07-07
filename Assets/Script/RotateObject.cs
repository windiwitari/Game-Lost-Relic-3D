using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Pengaturan Rotasi")]
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 100, 0); // Berputar 100 derajat per detik pada sumbu Y

    void Update()
    {
        // Memutar objek berdasarkan kecepatan, dikalikan dengan Time.deltaTime agar rotasi halus di semua FPS
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}