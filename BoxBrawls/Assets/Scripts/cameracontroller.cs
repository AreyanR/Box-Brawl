using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontroller : MonoBehaviour
{
    public GameObject player;
    public playercontroller playerScript;

    public GameObject opponent;
    public enemyscript opponentScript;

    private Vector3 offset;

    public float pulseFrequency = 2f;
    public float maxPulseAmplitude = 0.5f;
    public float maxNoiseAmplitude = 0.1f;

    void Start()
    {
        offset = transform.position - player.transform.position;

        // Get scripts
        playerScript = player.GetComponent<playercontroller>();
        opponentScript = opponent.GetComponent<enemyscript>();
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;

        if (playerScript != null && opponentScript != null)
        {
            float playerRatio = playerScript.health / playerScript.maxhealth;
            float opponentRatio = opponentScript.health / opponentScript.maxhealth;
            float minRatio = Mathf.Min(playerRatio, opponentRatio);

            float intensity = 1f - minRatio; // Lower health → higher intensity

            float pulse = Mathf.Sin(Time.time * pulseFrequency * Mathf.PI * 2) * maxPulseAmplitude * intensity;
            Vector3 randomNoise = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f) * maxNoiseAmplitude * intensity;

            transform.position += new Vector3(0f, pulse, 0f) + randomNoise;
        }
    }
}
