// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// using TMPro;

// public class ReactionManager : MonoBehaviour

// {

//     [Header("Prefabs")]

//     public GameObject greenBallPrefab;

//     public GameObject redBallPrefab;

//     [Header("Spawn Points")]

//     public Transform[] spawnPoints;

//     [Header("UI")]

//     public TMP_Text scoreText;

//     public TMP_Text timerText;

//     public TMP_Text stateText;

//     [Header("Arduino Serial")]

//     public SerialController serial;

//     [Header("Game Settings")]

//     public float gameTime = 60f;        // เวลาเล่นทั้งหมด (วินาที)

//     public float ballVisibleTime = 1f;  // เวลาที่ลูกบอลโผล่

//     private int score = 0;

//     private float timeLeft;

//     private bool gameRunning = false;

//     private GameObject currentBall;

//     private bool currentIsGreen = false;

//     private float currentSpawnTime = 0f;

//     private bool buttonPressedThisBall = false;

//     void Start()

//     {

//         timeLeft = gameTime;

//         UpdateUI();

//         StartCoroutine(GameLoop());

//     }

//     void Update()

//     {

//         if (!gameRunning) return;

//         timeLeft -= Time.deltaTime;

//         if (timeLeft <= 0f)

//         {

//             timeLeft = 0f;

//             EndGame();

//         }

//         if (timerText != null)

//         {

//             timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();

//         }

//     }

//     IEnumerator GameLoop()

//     {

//         gameRunning = true;

//         while (timeLeft > 0f)

//         {

//             SpawnRandomBall();

//             currentSpawnTime = Time.time;

//             buttonPressedThisBall = false;

//             float t = 0f;

//             while (t < ballVisibleTime && timeLeft > 0f)

//             {

//                 t += Time.deltaTime;

//                 yield return null;

//             }

//             DestroyCurrentBall();

//             // ถ้าเป็นลูกเขียวแต่ไม่กดเลย = Miss

//             if (!buttonPressedThisBall && currentIsGreen)

//             {

//                 if (stateText != null)

//                     stateText.text = "Miss!";

//                 if (serial != null)

//                     serial.SendMiss();

//             }

//             yield return new WaitForSeconds(0.3f);

//         }

//         EndGame();

//     }

//     void SpawnRandomBall()

//     {

//         DestroyCurrentBall();

//         if (spawnPoints == null || spawnPoints.Length == 0)

//         {

//             Debug.LogWarning("No spawn points assigned.");

//             return;

//         }

//         // สุ่มสี (50/50)

//         currentIsGreen = (Random.value > 0.5f);

//         // สุ่มตำแหน่ง

//         Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

//         GameObject prefab = currentIsGreen ? greenBallPrefab : redBallPrefab;

//         if (prefab == null)

//         {

//             Debug.LogWarning("Ball prefab is not assigned.");

//             return;

//         }

//         currentBall = Instantiate(prefab, sp.position, Quaternion.identity);

//         if (stateText != null)

//             stateText.text = "";

//     }

//     void DestroyCurrentBall()

//     {

//         if (currentBall != null)

//         {

//             Destroy(currentBall);

//             currentBall = null;

//         }

//     }

//     public void OnButtonPressed()

//     {

//         if (!gameRunning) return;

//         if (currentBall == null) return;

//         if (buttonPressedThisBall) return;

//         buttonPressedThisBall = true;

//         float reactionTimeMs = (Time.time - currentSpawnTime) * 1000f;

//         if (currentIsGreen)

//         {

//             score++;

//             if (stateText != null)

//                 stateText.text = "Hit! " + reactionTimeMs.ToString("F0") + " ms";

//             if (serial != null)

//                 serial.SendHit();

//         }

//         else

//         {

//             if (stateText != null)

//                 stateText.text = "Wrong! " + reactionTimeMs.ToString("F0") + " ms";

//             if (serial != null)

//                 serial.SendMiss();

//         }

//         UpdateUI();

//         DestroyCurrentBall();

//     }

//     void UpdateUI()

//     {

//         if (scoreText != null)

//             scoreText.text = "Score: " + score.ToString();

//         if (timerText != null)

//             timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();

//     }

//     void EndGame()

//     {

//         if (!gameRunning) return;

//         gameRunning = false;

//         DestroyCurrentBall();

//         if (stateText != null)

//             stateText.text = "Finish! Score: " + score.ToString();

//     }

// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReactionManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject greenBallPrefab;
    public GameObject redBallPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text stateText;

    [Header("Arduino Serial")]
    public SerialController serial;

    [Header("Game Settings")]
    public float gameTime = 60f;        // เวลาเล่นทั้งหมด (วินาที)
    public float ballVisibleTime = 1f;  // เวลาที่ลูกบอลโผล่

    [Header("Pressure Mode")]
    public bool enablePressureMode = true;   // เปิด/ปิดโหมดความกดดัน
    public int missStreakThreshold = 3;      // Miss ติดกันกี่ครั้งถึงจะกดดัน
    private int missStreak = 0;              // นับจำนวน Miss ติดกัน

    private int score = 0;
    private float timeLeft;
    private bool gameRunning = false;
    private GameObject currentBall;
    private bool currentIsGreen = false;
    private float currentSpawnTime = 0f;
    private bool buttonPressedThisBall = false;

    void Start()
    {
        timeLeft = gameTime;
        UpdateUI();
        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if (!gameRunning) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            EndGame();
        }

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();
        }
    }

    IEnumerator GameLoop()
    {
        gameRunning = true;

        while (timeLeft > 0f)
        {
            SpawnRandomBall();
            currentSpawnTime = Time.time;
            buttonPressedThisBall = false;

            float t = 0f;
            while (t < ballVisibleTime && timeLeft > 0f)
            {
                t += Time.deltaTime;
                yield return null;
            }

            // หมดเวลาลูกบอลลูกนี้
            DestroyCurrentBall();

            // ถ้าเป็นลูกเขียวแต่ไม่กดเลย = Miss
            if (!buttonPressedThisBall && currentIsGreen)
            {
                if (stateText != null)
                    stateText.text = "Miss!";

                RegisterMiss();
            }

            yield return new WaitForSeconds(0.3f);
        }

        EndGame();
    }

    void SpawnRandomBall()
    {
        DestroyCurrentBall();

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        // สุ่มสี (50/50)
        currentIsGreen = (Random.value > 0.5f);

        // สุ่มตำแหน่ง
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = currentIsGreen ? greenBallPrefab : redBallPrefab;

        if (prefab == null)
        {
            Debug.LogWarning("Ball prefab is not assigned.");
            return;
        }

        currentBall = Instantiate(prefab, sp.position, Quaternion.identity);

        if (stateText != null)
            stateText.text = "";
    }

    void DestroyCurrentBall()
    {
        if (currentBall != null)
        {
            Destroy(currentBall);
            currentBall = null;
        }
    }

    public void OnButtonPressed()
    {
        if (!gameRunning) return;
        if (currentBall == null) return;
        if (buttonPressedThisBall) return;

        buttonPressedThisBall = true;

        float reactionTimeMs = (Time.time - currentSpawnTime) * 1000f;

        if (currentIsGreen)
        {
            // กดถูก
            score++;
            missStreak = 0; // กดถูก รีเซ็ต Miss ติดกัน

            if (stateText != null)
                stateText.text = "Hit! " + reactionTimeMs.ToString("F0") + " ms";

            if (serial != null)
                serial.SendHit();
        }
        else
        {
            // กดโดนลูกแดง = ผิด
            if (stateText != null)
                stateText.text = "Wrong! " + reactionTimeMs.ToString("F0") + " ms";

            RegisterMiss();
        }

        UpdateUI();
        DestroyCurrentBall();
    }

    void RegisterMiss()
    {
        // ส่ง Miss ปกติก่อน
        if (serial != null)
            serial.SendMiss();

        // นับ Miss ติดกัน
        missStreak++;

        // ถ้าเปิด Pressure Mode และ Miss ติดกันถึงเกณฑ์
        if (enablePressureMode && missStreakThreshold > 0 && missStreak >= missStreakThreshold)
        {
            if (serial != null)
                serial.SendPressure();   // ส่งคำสั่งความกดดันไปที่ Arduino

            missStreak = 0; // รีเซ็ตนับใหม่
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();

        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();
    }

    void EndGame()
    {
        if (!gameRunning) return;

        gameRunning = false;
        DestroyCurrentBall();

        if (stateText != null)
            stateText.text = "Finish! Score: " + score.ToString();
    }
}
