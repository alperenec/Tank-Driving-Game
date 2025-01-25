using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    public Transform[] wheels; // 4 tekerlek (ön ve arka)
    public float rotationSpeed = 10f; // İleri-geri dönüş hızı
    public float steerAngle = 30f; // Direksiyon açısı (ön tekerlekler için)
    public CarController carController; // Aracın hızını almak için

    void Update()
    {
        float speed = carController.GetCurrentSpeed(); // Aracın mevcut hızını al

        // Tüm tekerleklerin ileri-geri dönmesi
        foreach (Transform wheel in wheels)
        {
            wheel.Rotate(Vector3.right, speed * rotationSpeed * Time.deltaTime);
        }

        // Tüm tekerleklerin y rotasyonu (yönlendirme)
        float input = Input.GetAxis("Horizontal");
        foreach (Transform wheel in wheels)
        {
            Vector3 localEulerAngles = wheel.localEulerAngles;
            localEulerAngles.y = input * steerAngle; // Sağa-sola yönlendirme
            wheel.localEulerAngles = localEulerAngles;
        }
    }
}
