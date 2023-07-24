using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static float maxHealth = 5f;
    public static int maxFuel = 3;
    public static float health = 5f;
    public static int fuel = 0;
    public static float speed = 1f;
    public static int attack = 1;
    public static float armor = 1f;
    public static int currLevel = -1;

    public static void refresh() {
        maxHealth = 5f;
        maxFuel = 10;
        health = 5f;
        fuel = 0;
        speed = 1f;
        attack = 1;
        armor = 1f;
    }

    public static void increaseSpeed() {
        speed *= 1.5f;
    }

    public static void decreaseHealth() {
        health -= 1.0f/armor;
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