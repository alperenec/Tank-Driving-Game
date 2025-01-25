using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{   
    
    
    public TextMeshProUGUI speedText; // UI'deki hız göstergesi
    public CarController carController; // Hız bilgisini almak için
    public float speedMultiplier = 3f; // Kod hızı ile oyun hızı arasındaki oran
    void Update()
    {
        // Kod hızını oyun hızına çevirmek için çarpan kullan
        float visualSpeed = Mathf.Abs(carController.GetCurrentSpeed()) * speedMultiplier;

        // Hızı yuvarlayarak göster (3'er 3'er artıyormuş gibi görünmesi için)
        speedText.text = Mathf.Round(visualSpeed).ToString("0") + " km/h"; // Hızı UI'ye yaz
    }
}
