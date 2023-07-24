using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void DemoLevelButton()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void Level1Button() {
        SceneManager.LoadScene(2);
    }
    public void Level2Button() {
        SceneManager.LoadScene(3);
    }
    public void Level3Button() {
        SceneManager.LoadScene(4);
    }
    public void Level4Button() {
        SceneManager.LoadScene(5);
    }
    public void Level5Button() {
        SceneManager.LoadScene(6);
    }
    public void Level6Button() {
        SceneManager.LoadScene(7);
    }
    public void Level7Button() {
        SceneManager.LoadScene(8);
    }
    public void Level8Button() {
        SceneManager.LoadScene(9);
    }
    public void Level9Button() {
        SceneManager.LoadScene(10);
    }
    public void Level10Button() {
        SceneManager.LoadScene(11);
    }
    public void Level11Button() {
        SceneManager.LoadScene(12);
    }
    public void Level12Button() {
        SceneManager.LoadScene(13);
    }
    public void RestartLevel() {
        switch(PlayerStats.currLevel) {
            case 0:
                DemoLevelButton();
                break;
            case 1:
                Level1Button();
                break;
            case 2:
                Level2Button();
                break;
            case 3:
                Level3Button();
                break;
            case 4:
                Level4Button();
                break;
            case 5:
                Level5Button();
                break;
            case 6:
                Level6Button();
                break;
            case 7:
                Level7Button();
                break;
            case 8:
                Level8Button();
                break;
            case 9:
                Level9Button();
                break;
            case 10:
                Level10Button();
                break;
            case 11:
                Level11Button();
                break;
            case 12:
                Level12Button();
                break;
        }
    }
}
