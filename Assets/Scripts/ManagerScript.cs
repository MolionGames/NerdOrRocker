using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour
{
    public PlayerController _player;

    public GameObject doctorPanel, normalPanel, rockStarPanel;

    public GameObject startButton;
    void Start()
    {
        doctorPanel.SetActive(false);
        normalPanel.SetActive(false);
        rockStarPanel.SetActive(false);
        startButton.SetActive(true);
    }

    public void StartGame()
    {
        _player.isPlaying = true;
        _player.anim.SetBool("isStarted", true);
        startButton.SetActive(false);
    }
    public void DoctorEnd()
    {
        doctorPanel.SetActive(true);
    }
    public void RockStarEnd()
    {
        rockStarPanel.SetActive(true);
    }
    public void NormalEnd()
    {
        normalPanel.SetActive(true);
    }
    public void NextButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
