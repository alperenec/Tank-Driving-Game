using UnityEngine;

public class LightControl : MonoBehaviour
{
    public Light directionalLight; // Güneş ışığı
    public float rotationSpeed = 1f; // Y rotasyonu için hız

    private float initialRotationX = -221.2f; // Başlangıç X rotasyonu
    private float initialRotationY = 1032.7f; // Başlangıç Y rotasyonu
    private float currentRotationY; // Mevcut Y rotasyonu

    void Start()
    {
        // Y rotasyonunu başlangıç pozisyonuna ayarla
        currentRotationY = initialRotationY;

        // Başlangıç rotasyonunu uygula
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(
            initialRotationX, // X rotasyonu
            currentRotationY, // Y rotasyonu
            0f // Z rotasyonu sabit
        ));
    }

    void Update()
    {
        // Y rotasyonunu artır
        currentRotationY += rotationSpeed * Time.deltaTime;

        // Yeni rotasyonu uygula
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(
            initialRotationX, // X rotasyonu sabit kalır
            currentRotationY, // Y rotasyonu artar
            0f // Z rotasyonu sabit kalır
        ));
    }
}
