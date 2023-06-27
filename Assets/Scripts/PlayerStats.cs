using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static int maxHealth = 5;
    public static int maxFuel = 3;
    public static int health = 5;
    public static int fuel = 0;
    public static float speed = 1f;
    
    public static void refresh() {
        maxHealth = 5;
        maxFuel = 3;
        health = 5;
        fuel = 0;
        speed = 1f;
    }

    public static void decreaseHealth() {
        --health;
    }

    public static bool isDeath() {
        return health == 0;
    }

    public static void increaseHealth() {
        if (health < maxHealth) ++health;
    }

    public static void restartFuel() {
        fuel = 0;
    }

    public static void increaseFuel() {
        fuel += fuel == maxFuel ? 0 : 1;
    }
}