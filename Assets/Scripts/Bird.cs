using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Bird : MonoBehaviour
{
    private Rigidbody2D body;
    public float score { set; get; }

    private float distanceUp;
    private float distanceDown;
    private float distanceForward;

    private LayerMask topCollider;
    private LayerMask bottomCollider;
    private LayerMask forwardCollider;

    public float weightF { get; set; }
    public float weightU { get; set; }
    public float weightD { get; set; }
    public float bias { get; set; }

    public Boolean dead;

    void Awake()
    {
        score = 0f;
        body = GetComponent<Rigidbody2D>();
        topCollider = LayerMask.NameToLayer("topCollider");
        bottomCollider = LayerMask.NameToLayer("bottomCollider");
        forwardCollider = LayerMask.NameToLayer("forwardCollider");
        dead = false;
    }

    void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.W)) { Flap(); }
        
        RaycastHit2D hit;
        distanceForward = forwardColliderD();
        distanceDown = bottomColliderD();
        distanceUp = topColliderD();

        //Debug.Log(distanceForward + " " + distanceUp + " " + distanceDown);
        score += 0.01f;

        float FTotal = distanceForward * weightF;
        float UTotal = distanceUp * weightU;
        float DTotal = distanceDown * weightD;
        float total = FTotal + UTotal + DTotal + bias;
        float choice = math.tanh(total);

        if(choice > 0.5f)
        {
            Flap();
            //Debug.Log("flapped");
        }
    }


    void Flap()
    {
        body.linearVelocity = new Vector2(0, 0);
        body.AddForce(new Vector2(0, 16), ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Pipe" && dead == false )
        {
            float finalScore = score;
            //Debug.Log(finalScore);
            dead = true;
            Destroy(gameObject);
        }
    }

    private float topColliderD()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Vector2.up, Mathf.Infinity, 1 << topCollider);
        if (hit)
        {
            return hit.distance;
        } else
        {
            hit = Physics2D.Raycast(new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Vector2.down, Mathf.Infinity, 1 << topCollider);
            if (hit)
            {
                return -hit.distance;
            } else
            {
                return 0f;
            }
        }
    }
    private float bottomColliderD()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Vector2.down, Mathf.Infinity, 1 << bottomCollider);
        if (hit)
        {
            return hit.distance;
        }
        else
        {
            hit = Physics2D.Raycast(new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Vector2.up, Mathf.Infinity, 1 << bottomCollider);
            if (hit)
            {
                return -hit.distance;
            }
            else
            {
                return 0f;
            }
        }
    }
    private float forwardColliderD()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Vector2.right, Mathf.Infinity, 1 << forwardCollider);
        if (hit)
        {
            return hit.distance;
        }
        else
        {
            return 0f;
        }
    }
}
