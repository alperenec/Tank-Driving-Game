using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
//road
public class CarController : MonoBehaviour
{

    public float WaitingSeconds = 0.5f;
    public float maxSpeed = 42f;
    public float acceleration = 12f;
    public float deceleration = 10f;
    public float brakePower = 16f;
    public float turnSpeed = 15f;
    public float maxTurnAngle = 1f;

    private float currentSpeed = 0f;
    private float currentTurnAngle = 0f;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 lastCheckpointPosition;
    private Quaternion lastCheckpointRotation;
    public string roadTag = "Road";
    public string checkpointTag = "Checkpoint";

    private bool[] checkpointsVisited;
    public int totalCheckpoints = 4;
    public Slider progressSlider;
    private int checkpointsReached = 0;
    public GameObject winText;
    private int nextCheckpointIndex = 0;
    public TextMeshProUGUI progressMessageText;

    // Timer
    public TextMeshProUGUI timerText;
    private float countdownTime = 120f;
    private bool isGameOver = false;
    private bool isPaused = false;
    public TextMeshProUGUI gameOverText; // Game Over yazısı için referans

    // Settings Menu
    public GameObject settingsMenu;

    private PlayerHealth playerHealth; // Sağlık sistemine erişim


    [System.Obsolete]
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        lastCheckpointPosition = startPosition;
        lastCheckpointRotation = startRotation;
        playerHealth = FindObjectOfType<PlayerHealth>(); // PlayerHealth script'ini bul

        checkpointsVisited = new bool[totalCheckpoints];

        if (progressSlider != null)
        {
            progressSlider.maxValue = totalCheckpoints;
            progressSlider.value = 0;
            progressSlider.interactable = false;
        }

        if (winText != null)
        {
            winText.SetActive(false);
        }

        if (progressMessageText != null)
        {
            progressMessageText.gameObject.SetActive(false);
        }

        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }

        StartTimer();
    }

    void Update()
    {

        
        
        if (isGameOver || isPaused) return;

        float input = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakePower * Time.deltaTime);
        }
        else if (input != 0)
        {
            currentSpeed += input * acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        float targetTurnAngle = Input.GetAxis("Horizontal") * maxTurnAngle;
        currentTurnAngle = Mathf.Lerp(currentTurnAngle, targetTurnAngle, turnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, currentTurnAngle, 0) * transform.rotation;

        UpdateTimer();

        
    }



public float GetCurrentSpeed()
{
    return currentSpeed; // Anlık hızı döndür
}

public void SetCurrentSpeed(float newSpeed)
{
    currentSpeed = Mathf.Clamp(newSpeed, 0, maxSpeed); // Yeni hızı güncelle, maxSpeed sınırını aşma
}



private bool isOnTerrain = false; // Terrain üzerinde olup olmadığını kontrol etmek için
private bool waitingForRoadContact = false; // 3 saniye kontrolü için
private Coroutine terrainCheckCoroutine; // Coroutine referansı
public string terrainTag = "Terrain"; // Terrain objesinin tag'i

void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag(terrainTag) && !isOnTerrain) // İlk kez terrain'e temas ediliyorsa
    {
        Debug.Log("Terrain'e temas edildi.");
        isOnTerrain = true;
        waitingForRoadContact = true;
        terrainCheckCoroutine = StartCoroutine(CheckIfStillOnTerrain());
    }
    else if (collision.gameObject.CompareTag(roadTag)) // Road ile temas edilirse
    {
        Debug.Log("Road'a temas edildi. Işınlama iptal ediliyor.");
        ResetTerrainCheck(); // Terrain kontrolünü sıfırla
    }
}

void OnCollisionExit(Collision collision)
{
    if (collision.gameObject.CompareTag(terrainTag)) // Terrain'den çıkış kontrolü
    {
        Debug.Log("Terrain'den çıkıldı.");
        isOnTerrain = false; // Terrain'de olmadığını kaydet
    }
}


IEnumerator CheckIfStillOnTerrain()
{
    yield return new WaitForSeconds(WaitingSeconds); 

    if (waitingForRoadContact && isOnTerrain) 
    {
        Debug.Log("3 saniye boyunca terrain'de kalındı. Checkpoint'e ışınlanıyor.");
        ReturnToCheckpoint();
    }

    ResetTerrainCheck(); // Kontrolü sıfırla
}

void ResetTerrainCheck()
{
    if (terrainCheckCoroutine != null)
    {
        StopCoroutine(terrainCheckCoroutine); // Coroutine'i durdur
        terrainCheckCoroutine = null;
    }

    waitingForRoadContact = false; // Bekleme durumu sıfırlanır
    isOnTerrain = false; // Terrain'den çıkış tamamlandı
}





    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(checkpointTag))
        {
            int checkpointIndex = int.Parse(other.name.Substring(1)) - 1;

            if (checkpointIndex == nextCheckpointIndex)
            {
                checkpointsVisited[checkpointIndex] = true;
                checkpointsReached++;
                Debug.Log($"{other.name}'den geçtiniz!");

                if (progressSlider != null)
                {
                    progressSlider.value = checkpointsReached;
                }

                nextCheckpointIndex++;

                if (checkpointIndex == totalCheckpoints - 1)
                {
                    WinGame();
                }

                ShowProgressMessage(checkpointIndex);

                lastCheckpointPosition = transform.position;
                lastCheckpointRotation = transform.rotation;
            }
        }
    }

    void ShowProgressMessage(int checkpointIndex)
    {
        if (progressMessageText != null)
        {
            progressMessageText.gameObject.SetActive(true);

            switch (checkpointIndex)
            {
                case 0:
                    progressMessageText.text = "Yolun 1/4'ünü tamamladınız!";
                    break;
                case 1:
                    progressMessageText.text = "Yolun 1/2'sini tamamladınız!";
                    break;
                case 2:
                    progressMessageText.text = "Yolun 3/4'ünü tamamladınız!";
                    break;
                case 3:
                    progressMessageText.text = "";
                    break;
            }

            StartCoroutine(HideProgressMessageAfterSeconds(2f));
        }
    }

    IEnumerator HideProgressMessageAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (progressMessageText != null)
        {
            progressMessageText.gameObject.SetActive(false);
        }
    }

    void ReturnToCheckpoint()
{
    Debug.Log("Yol dışına çıktınız. Son checkpoint pozisyonuna dönülüyor...");
    transform.position = lastCheckpointPosition;
    transform.rotation = lastCheckpointRotation;
    currentSpeed = 0f;

    // Can azaltma işlemi
    if (playerHealth != null)
    {
        playerHealth.TakeDamage();
    }
}


    // Timer Functions
    void StartTimer()
    {
        if (timerText != null)
        {
            timerText.text = countdownTime.ToString("0");
        }
    }

    void UpdateTimer()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = Mathf.Ceil(countdownTime).ToString();
            }
        }
        else if (!isGameOver)
        {
            GameOver();
        }
    }

    void WinGame()
{
    Debug.Log("Tebrikler! Kazandınız!");
    if (winText != null)
    {
        winText.SetActive(true);
        StartCoroutine(HideWinMessageAfterSeconds(3f)); // 3 saniye sonra gizle
    }
    Time.timeScale = 0f;
    isGameOver = true;
}

void GameOver()
{
    Debug.Log("Süre doldu! Oyun bitti.");
    isGameOver = true;
    Time.timeScale = 0f;
    if (gameOverText != null)
    {
        gameOverText.gameObject.SetActive(true);
        StartCoroutine(HideGameOverMessageAfterSeconds(3f)); // 3 saniye sonra gizle
    }
}

    private string HideGameOverMessageAfterSeconds(float v)
    {
        throw new NotImplementedException();
    }

    IEnumerator HideWinMessageAfterSeconds(float seconds)
{
    yield return new WaitForSeconds(seconds);
    if (winText != null)
    {
        winText.SetActive(false);
    }
}



    // Settings Menu Functions
    public void ToggleSettingsMenu()
    {
        if (settingsMenu != null)
        {
            bool isActive = settingsMenu.activeSelf;
            settingsMenu.SetActive(!isActive);

            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
        }
    }

    // Önceki zorluk seviyesini kaydetmek için bir değişken
private float previousDifficultyTime = 100f;

// Zorluk seviyelerini ayarlama ve önceki zorluk süresini kaydetme
public void SetTimerEasy()
{
    previousDifficultyTime = 240f; // Kolay seviyenin süresi
    RestartGameWithNewDifficulty();
}

public void SetTimerMedium()
{
    previousDifficultyTime = 150f; // Orta seviyenin süresi
    RestartGameWithNewDifficulty();
}

public void SetTimerHard()
{
    previousDifficultyTime = 80f; // Zor seviyenin süresi
    RestartGameWithNewDifficulty();
}

// Yeniden başlatırken önceki zorluk seviyesini kullan
public void RestartGame()
{
    countdownTime = previousDifficultyTime; // Önceki zorluk seviyesini sıfırla
    isPaused = false; // Oyunun duraklatılmadığından emin olun
    Time.timeScale = 1f; // Oyun hızını sıfırdan çıkar
    RestartGameWithTimer();    
}

// Oyunu sıfırlayan ana işlev
// Oyunu sıfırlayan ana işlev
void RestartGameWithTimer()
{
    Time.timeScale = 1f;
    isGameOver = false;
    checkpointsReached = 0;
    nextCheckpointIndex = 0;

    // Aracın başlangıç konumuna dön
    transform.position = startPosition;
    transform.rotation = startRotation;

    // Son checkpoint konumunu sıfırla
    lastCheckpointPosition = startPosition;
    lastCheckpointRotation = startRotation;

    // Progress bar sıfırla
    if (progressSlider != null)
    {
        progressSlider.value = 0;
    }

    // "Game Over" yazısını gizle
    if (gameOverText != null)
    {
        gameOverText.gameObject.SetActive(false);
    }

    // Menü kapatılsın
    if (settingsMenu != null)
    {
        settingsMenu.SetActive(false); // Menuyu gizle
    }

    // Kalpleri sıfırla
    if (playerHealth != null)
    {
        playerHealth.ResetHealth();
    }

    // Hız sıfırlama
    SetCurrentSpeed(0f);

    // Kalp ve Power-Up nesnelerini yeniden yükle
    RespawnCollectibles();

    // Yeni geri sayımı başlat
    StartTimer();
}

void RespawnCollectibles()
{
    foreach (Transform collectible in GameObject.Find("CoinsHeartsOnRoad").transform)
    {
        collectible.gameObject.SetActive(true); // Tüm kalp ve power-up'ları yeniden etkinleştir
    }
}





// Yeni zorluk seviyesi ayarlandığında oyunu sıfırla
void RestartGameWithNewDifficulty()
{
    countdownTime = previousDifficultyTime; // Yeni zorluk seviyesini ayarla
    isPaused = false; // Oyunun duraklatılmadığından emin olun
    Time.timeScale = 1f; // Oyun hızını sıfırdan çıkar
    RestartGameWithTimer(); // Oyun elemanlarını sıfırla
}




}
