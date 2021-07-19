using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;
    private float bottomBound = -10.0f;
    public float speed = 3f;
  
    void Start()
    {
        enemyRb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        switch (Save.difficultyLevel)
        {
            case "Easy":
              speed = 1f;
              break;
            case "Normal":
              speed = 2f;
              break;
            case "Hard":
              speed = 3f;
              break;
            default:
              break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        enemyRb.AddForce(lookDirection * speed);

        if (transform.position.y < bottomBound)
        {
            Destroy(gameObject);
        }

        if (player.transform.position.y < -1f)
        {
            enemyRb.velocity = Vector3.zero;
        }
    }
}
