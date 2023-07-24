using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag == "Health")
        {
            PlayerStats.increaseHealth();
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Speed")
        {
            PlayerStats.increaseSpeed();
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Attack") {
            PlayerStats.attack++;
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Armor") {
            PlayerStats.armor++;
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Random") {
            System.Random rnd = new System.Random();
            int x = rnd.Next(4);
            switch (x) {
                case 0:
                    PlayerStats.increaseHealth();
                    break;
                case 1:
                    PlayerStats.increaseSpeed();
                    break;
                case 2:
                    PlayerStats.armor++;
                    break;
                case 3:
                    PlayerStats.attack++;
                    break;
            }
            Destroy(collision.gameObject);
        }
    }
}
