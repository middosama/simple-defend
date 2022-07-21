using System.Collections;
using UnityEngine;


public class Character : MonoBehaviour
{
    Rigidbody2D rb;
    float maxSpeed;
    float minSpeed;
    float health;
    // Use this for initialization
    void Start()
    {
        // set all properties random
        rb = GetComponent<Rigidbody2D>();
        maxSpeed = Random.Range(1, 10);
        minSpeed = Random.Range(1, 10);
        health = Random.Range(1, 10);
        // set all properties to 0
        rb.velocity = Vector2.zero;
        

    }

    void Update()
    {
        
        // make character move by input key
        
        
        




    }


}
