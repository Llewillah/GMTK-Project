using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Timer : MonoBehaviour
{
    [Header("Timer stuff")]
    [SerializeField] private float timeLimit;
    [SerializeField] private float eventPercent1 = 0.0f;
    [SerializeField] private float eventPercent2 = 0.0f;
    [SerializeField] private float eventPercent3 = 0.0f;
    [SerializeField] private Vector2 spawnLocation;
    [SerializeField] private TMP_Text timerDisplay;
    [SerializeField] private GameObject bloodParticles;
    private float timeRemaining;
    private bool timerRunning = true;
    private bool event1Triggerd = false;
    private bool event2Triggerd = false;
    private bool event3Triggerd = false;

    [Header("Event Related Things")]
    [SerializeField] private Light2D playerLight;
    [SerializeField] private float lowestLightRadius;
    [SerializeField] private bool lightTimer = true;
    private float normalRadius; //for the light radius
    [SerializeField] private AudioSource damageTakenAudio;
    [SerializeField] private GameObject HADESISUPONUS;
    [SerializeField] private GameObject respawnUI;

    private void Start()
    {
        spawnLocation = transform.position;

        timeRemaining = timeLimit;
        DisplayTime(timeRemaining);

        normalRadius = playerLight.pointLightOuterRadius + lowestLightRadius;
    }

    private void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if ((timeRemaining / timeLimit) <= eventPercent1 && event1Triggerd == true)
            {
                event1Triggerd = true;
                //event 1 stuff here
            }

            if ((timeRemaining / timeLimit) <= eventPercent2 && event2Triggerd == true)
            {
                event2Triggerd = true;
                //event 2 stuff here
            }

            if ((timeRemaining / timeLimit) <= eventPercent3 && event3Triggerd == true)
            {
                event3Triggerd = true;
                //event 3 stuff here
            }

            if (lightTimer == true)
            {
                playerLight.pointLightOuterRadius = ((timeRemaining / timeLimit) * normalRadius) + lowestLightRadius;
            }

            DisplayTime(timeRemaining);

            //player death
            if (timeRemaining < 0)
            {
                //death stuff here
                gameObject.GetComponent<PlayerMovement>().SetCanMove(false);
                Instantiate(HADESISUPONUS, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 3, gameObject.transform.position.z), Quaternion.identity);
                timerRunning = false;
                StartCoroutine(ShowRespawnScreen());
            }
        }
    }

    public void ReduceTimer(float amount)
    {
        timeRemaining -= amount;
        Instantiate(bloodParticles, gameObject.transform.position, Quaternion.identity); //spawn the particle for taking damage
        damageTakenAudio.Play(); //play sfx when taking damage
    }

    private void DisplayTime(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void PlayerRespawn()
    {
        timerRunning = true;
        gameObject.GetComponent<PlayerMovement>().SetCanMove(true);
        gameObject.transform.position = spawnLocation;
        playerLight.pointLightOuterRadius = normalRadius;
        timeRemaining = timeLimit;
        respawnUI.SetActive(false);
        GameObject findingHades = GameObject.FindGameObjectWithTag("BADEVILGUY");
        Destroy(findingHades);
    }

    IEnumerator ShowRespawnScreen()
    {
        yield return new WaitForSeconds(3);
        respawnUI.SetActive(true);
    }
}
