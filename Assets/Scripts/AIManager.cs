using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public GameObject birdObj;

    public Bird[] newBirdArray;
    public Bird[] oldBirdArray;

    private Bird bird;
    private GameObject birdOb;
    private Bird bestBird;
    private Bird cBird;

    public GameManager manager;

    private int gen = 0;


    private int genSize = 100;

    public bool alive;
    void Start()
    {
        newBirdArray = new Bird[genSize];
        oldBirdArray = new Bird[genSize];
    }

    public void startGen()
    {
        alive = true;
        for(int i = 0; i < genSize; i++)
        {
            GameObject birdOb = Instantiate(birdObj, new Vector2(-7.5f, 0), transform.rotation);
            Bird bird = birdOb.GetComponent<Bird>();
            bird.weightD = UnityEngine.Random.Range(-20.00f, 20.00f);
            bird.weightU = UnityEngine.Random.Range(-20.00f, 20.00f);
            bird.weightF = UnityEngine.Random.Range(-20.00f, 20.00f);
            bird.bias = UnityEngine.Random.Range(-1000.00f, 1000.00f);
            newBirdArray[i] = bird;
        }

        //Debug.Log(newBirdArray.Length);
        Debug.Log(bestBird.weightF + " : " + bestBird.weightU + " : " + bestBird.weightD + " : " + bestBird.bias);
    }

    private void Update()
    {
        if (!checkLife())
        {
            //Debug.Log("DEAD");
            gen += 1;
            setNewGen();
        }
    }

    private bool checkLife()
    {
        bool alive = false;
        for (int i = 0; i < genSize; i++)
        {
            if (newBirdArray[i].dead == false)
            {
                alive = true;
            }
        }
        return alive;
    }

    private void setNewGen()
    {
        bestBird = newBirdArray[0];
        for (int i = 0; i < genSize; i++)
        {
            if(newBirdArray[i].score > bestBird.score)
            {
                bestBird = newBirdArray[i];
            }
        }

        DeleteAll();

        if (bestBird.score < 2f && gen < 3)
        {
            startGen();
        }
        else
        {
            oldBirdArray = newBirdArray;
            for (int i = 0; i < genSize; i++)
            {
                if (i == 0)
                {
                    Destroy(newBirdArray[i]);
                    birdOb = Instantiate(birdObj, new Vector2(-7.5f, 0), transform.rotation);
                    bird = birdOb.GetComponent<Bird>();
                    bird.weightD = bestBird.weightD;
                    bird.weightU = bestBird.weightU;
                    bird.weightF = bestBird.weightF;
                    bird.bias = bestBird.bias;
                    newBirdArray[i] = bird;
                }
                else if (i < genSize/5)
                {
                    Destroy(newBirdArray[i]);
                    birdOb = Instantiate(birdObj, new Vector2(-7.5f, 0), transform.rotation);
                    bird = birdOb.GetComponent<Bird>();
                    bird.weightD = bestBird.weightD += UnityEngine.Random.Range(-0.0300f, 0.0300f);
                    bird.weightU = bestBird.weightU += UnityEngine.Random.Range(-0.0300f, 0.0300f);
                    bird.weightF = bestBird.weightF += UnityEngine.Random.Range(-0.0300f, 0.030f);
                    bird.bias = bestBird.bias += UnityEngine.Random.Range(-2.00f, 2.00f);
                    newBirdArray[i] = bird;
                }
                else if (i < genSize*0.8f)
                {
                    oldBirdArray = sortScore(oldBirdArray);
                    cBird = oldBirdArray[UnityEngine.Random.Range(0, genSize/2)];
                    Destroy(newBirdArray[i]);
                    birdOb = Instantiate(birdObj, new Vector2(-7.5f, 0), transform.rotation);
                    bird = birdOb.GetComponent<Bird>();
                    bird.weightD = cBird.weightD += UnityEngine.Random.Range(-0.22500f, 0.22500f);
                    bird.weightU = cBird.weightU += UnityEngine.Random.Range(-0.22500f, 0.22500f);
                    bird.weightF = cBird.weightF += UnityEngine.Random.Range(-0.22500f, 0.22500f);
                    bird.bias = cBird.bias += UnityEngine.Random.Range(-5.500f, 5.500f);
                    newBirdArray[i] = bird;
                }
                else
                {
                    oldBirdArray = sortScore(oldBirdArray);
                    cBird = oldBirdArray[UnityEngine.Random.Range(0, 50)];
                    Destroy(newBirdArray[i]);
                    birdOb = Instantiate(birdObj, new Vector2(-7.5f, 0), transform.rotation);
                    bird = birdOb.GetComponent<Bird>();
                    bird.weightD = UnityEngine.Random.Range(-20.00f, 20.00f);
                    bird.weightU = UnityEngine.Random.Range(-20.00f, 20.00f);
                    bird.weightF = UnityEngine.Random.Range(-20.00f, 20.00f);
                    bird.bias = UnityEngine.Random.Range(-100.00f, 100.00f);
                    newBirdArray[i] = bird;
                }
            }
        }
    }

    public void DeleteAll()
    {
        foreach (GameObject o in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            if (o.name == "Pipes 1(Clone)" && o.transform.position.x < -6f)
            {
                Destroy(o);
            }
        }
    }

    private Bird[] sortScore(Bird[] oldBirdArray)
    {
        //Bird[] newArray = new Bird[genSize];
        Bird temp;
        for (int j = 0; j <= oldBirdArray.Length - 2; j++)
        {
            for (int i = 0; i <= oldBirdArray.Length - 2; i++)
            {
                if (oldBirdArray[i].score > oldBirdArray[i + 1].score)
                {
                    temp = oldBirdArray[i + 1];
                    oldBirdArray[i + 1] = oldBirdArray[i];
                    oldBirdArray[i] = temp;
                }
            }
        }
        return oldBirdArray;
    }
}
