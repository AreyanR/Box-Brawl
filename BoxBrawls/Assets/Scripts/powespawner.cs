using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powespawner : MonoBehaviour
{
    // This script handles the spawning of powerups in the game.
    // It randomly spawns powerups at specified intervals and ensures they are not spawned when the player or enemy is inactive.
    public GameObject[] myobject; 
    float timePassed = 0f; 

    public GameObject player; 
    public GameObject enemy; 

    private bool shouldSpawnPowerups = true; 

    void Update()
    {
        if (shouldSpawnPowerups)
        {
            timePassed += Time.deltaTime; 

            if (timePassed > 3.0f)
            {
                int randomIndex = Random.Range(0, myobject.Length); 
                Vector3 randomspawnposition = new Vector3(Random.Range(-9, 9), 1, Random.Range(-9, 9)); // Random spawn location

                Instantiate(myobject[randomIndex], randomspawnposition, Quaternion.identity); // Spawn it

                timePassed = 0f; 
            }
        }

        
        if (!player.activeSelf || !enemy.activeSelf)
        {
            shouldSpawnPowerups = false;
        }
    }
}
