using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class AmbushScene : MonoBehaviour
{
    //this is how we going to control the flow of the ambush event.

    [SerializeField] Transform respawnCenter;
    [SerializeField] Transform respawnRight;
    [SerializeField] Transform respawnLeft;
    [SerializeField] Door door1;
    [SerializeField] Door door2;
    [SerializeField] EnemyBase template;
    [SerializeField] GameObject projectilTemplate;
    [SerializeField] Transform rainReference;
    [SerializeField] int rainDuration;
    int currentRainDuration;

    bool done;
    //when we restore the state we restart everything unless it was done.

    public void StartAmbush()
    {
        //lock the player and close the door.
        //then do a little narration.
        PlayerHandler.instance.eventPlayerDeath += CancelScene;
        StartCoroutine(StartProcess());
    }

    void CancelScene()
    {
        Observer.instance.OnNarration();
        StopAllCoroutines();
        PlayerHandler.instance.eventPlayerDeath -= CancelScene;
    }

    IEnumerator StartProcess()
    {
        //close one door.
        door1.CloseDoor();

        yield return new WaitForSeconds(2);
        //then we have a narration
        Observer.instance.OnNarration("Welcome to the arena of the great apartment");
        yield return new WaitForSeconds(6.5f);
        Observer.instance.OnNarration("Unleash the beasts!");
        yield return new WaitForSeconds(5.5f);
        Observer.instance.OnNarration();

        //start spawning.
        //create a list of all the spawned so we can delete them on save.

        for (int i = 0; i < 10; i++)
        {
            int random = Random.Range(0, 3);

            Debug.Log("this is the random " + random);

            if(random == 0)
            {
                //spawn in the left
                SpawnZombie(respawnLeft);
            }
            if(random == 1)
            {
                //spawn in teh center
                SpawnZombie(respawnCenter);
            }
            if(random == 2)
            {
                //spawn in the right
                SpawnZombie(respawnRight);
            }

            float randomWaitTimer = Random.Range(1, 2.5f);
            yield return new WaitForSeconds(randomWaitTimer);

        }

        //if the player is killed the process ends.
        yield return new WaitForSeconds(8);

        Observer.instance.OnNarration("He survived the rain of beats.");
        yield return new WaitForSeconds(3.5f);
        Observer.instance.OnNarration("Now he must face the rain of bullets");
        //activate two turrets.

        //actually we will just 

        while(rainDuration > currentRainDuration)
        {
            currentRainDuration += 1;

            SpawnBullets(Random.Range(2, 6));

            float duration = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(duration);           
        }

        yield return new WaitForSeconds(3);       

        Observer.instance.OnNarration("He has survived!");
        yield return new WaitForSeconds(5);
        Observer.instance.OnNarration("He deserves to live in the apartment");
        yield return new WaitForSeconds(5);
        Observer.instance.OnNarration("");

    }

    void SpawnZombie(Transform spawnPos)
    {
        EnemyBase newObject = Instantiate(template, spawnPos.position, Quaternion.identity);

    }

    void SpawnBullets(int amount)
    {
        //create along the line.
        //
       
        Vector3 offset = new Vector3(0, Random.Range(-8, 8), 0);
        GameObject newObject = Instantiate(projectilTemplate, rainReference.position + offset, Quaternion.identity);
    }

}
