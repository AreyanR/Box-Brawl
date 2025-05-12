using UnityEngine;
using UnityEngine.UI;

public class StadiumPulseController : MonoBehaviour
{
    // This script controls the color and intensity of a UI element based on the health of two players.
    public playercontroller playerScript;
    public enemyscript opponentScript;

    public Image fillImage;
    public Color lowColor = Color.green;
    public Color midColor = Color.yellow;
    public Color highColor = Color.red;

    void Update()
    {
        if (playerScript != null && opponentScript != null)
        {
            float playerRatio = playerScript.health / playerScript.maxhealth;
            float opponentRatio = opponentScript.health / opponentScript.maxhealth;
            float minRatio = Mathf.Min(playerRatio, opponentRatio);

            float intensity = 1f - minRatio;

            fillImage.fillAmount = intensity;

            if (intensity < 0.5f)
            {
                float t = intensity / 0.5f;
                fillImage.color = Color.Lerp(lowColor, midColor, t);
            }
            else
            {
                float t = (intensity - 0.5f) / 0.5f;
                fillImage.color = Color.Lerp(midColor, highColor, t);
            }
        }
    }
}
