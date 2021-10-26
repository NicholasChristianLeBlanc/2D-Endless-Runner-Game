using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    [SerializeField] private GameObject[] boundaries;
    [SerializeField] private GameObject killBlock;
    [SerializeField] private GameObject[] spawnpoints;
    
    private List<int> counters = new List<int>();
    private List<int> spawnCooldowns = new List<int>();
    private GameObject selectedSpawnpoint, secondarySpawnpoint, spawnedKillBlock, secondarySpawnedKillBlock;
    private int selectedValue;



    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject boundary in boundaries)
        {
            counters.Add(0);
            spawnCooldowns.Add((int)Random.Range(150, 500));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int i = 0;

        foreach (GameObject boundary in boundaries)
        {
            counters[i]++;

            if (counters[i] >= spawnCooldowns[i])
            {
                selectedValue = (int)Random.Range(0, spawnpoints.Length);

                if ((int)Random.Range(0, 35) == 1) // 1 in 34 chance for another block to spawn in one of the spawnpoints directly next to the selected one 
                {
                    if ((int)Random.Range(0, 2) == 1) // returns 0 or 1 for the spawnpoint before or after the selected one
                    {
                        if (selectedValue - 1 < 0
                            || spawnpoints[selectedValue - 1].gameObject.transform.parent.gameObject != spawnpoints[selectedValue].gameObject.transform.parent.gameObject
                            || spawnpoints.GetValue(selectedValue - 1) == null) // if there is no spawnpoint BEFORE the selected one, spawn a block at the spawnpoint AFTER the selected one
                        {
                            if ((selectedValue + 1 < spawnpoints.Length
                                || spawnpoints[selectedValue + 1].gameObject.transform.parent.gameObject == spawnpoints[selectedValue].gameObject.transform.parent.gameObject)
                                && spawnpoints.GetValue(selectedValue + 1) != null)
                            {
                                secondarySpawnpoint = (GameObject)spawnpoints.GetValue(selectedValue + 1);
                            }
                        }
                        else
                        {
                            secondarySpawnpoint = (GameObject)spawnpoints.GetValue(selectedValue - 1);
                        }
                    }
                    else
                    {
                        if (selectedValue + 1 >= spawnpoints.Length
                            || spawnpoints[selectedValue + 1].gameObject.transform.parent.gameObject != spawnpoints[selectedValue].gameObject.transform.parent.gameObject
                            || spawnpoints.GetValue(selectedValue + 1) == null) // if there is no spawnpoint AFTER the selected one, spawn a block at the spawnpoint Before the selected one
                        {
                            if ((selectedValue - 1 >= 0
                                || spawnpoints[selectedValue - 1].gameObject.transform.parent.gameObject == spawnpoints[selectedValue].gameObject.transform.parent.gameObject)
                                && spawnpoints.GetValue(selectedValue - 1) != null)
                            {
                                secondarySpawnpoint = (GameObject)spawnpoints.GetValue(selectedValue - 1);
                            }
                        }
                        else
                        {
                            secondarySpawnpoint = (GameObject)spawnpoints.GetValue(selectedValue + 1);
                        }
                    }

                    if (secondarySpawnpoint != null)
                    {
                        secondarySpawnedKillBlock = Instantiate(killBlock, secondarySpawnpoint.transform.position, secondarySpawnpoint.transform.rotation);
                        secondarySpawnedKillBlock.GetComponent<KillBlocks>().SetStatic(false);
                        secondarySpawnedKillBlock.GetComponent<KillBlocks>().CompareBoundaries(boundaries);
                        secondarySpawnedKillBlock.SetActive(true);
                    }
                }

                selectedSpawnpoint = (GameObject)spawnpoints.GetValue(selectedValue);

                spawnedKillBlock = Instantiate(killBlock, selectedSpawnpoint.transform.position, selectedSpawnpoint.transform.rotation);
                spawnedKillBlock.GetComponent<KillBlocks>().SetStatic(false);
                spawnedKillBlock.GetComponent<KillBlocks>().CompareBoundaries(boundaries);
                spawnedKillBlock.SetActive(true);

                spawnCooldowns[i] = (int)Random.Range(150, 500);
                counters[i] = 0;
            }

            i = 0;
        }
    }
}