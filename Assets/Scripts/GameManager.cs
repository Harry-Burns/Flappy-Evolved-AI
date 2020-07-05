using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public float waitTime { get; set; }
    public IEnumerator coroutine;
    public AIManager aiManager;

    public GameObject pipes;
    void Start()
    {
        waitTime = 2.7f;
        coroutine = SpawnDelay();
        StartCoroutine(coroutine);
        aiManager.startGen();
    }

    void FixedUpdate()
    {
        
    }

    IEnumerator SpawnDelay()
    {
        while (true)
        {
            float height = Random.Range(2.88f, -2.88f);
            height = height - (height % 0.1f);
            GameObject pipe = Instantiate(pipes, new Vector2(11f, height), quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        } 
    }
}
