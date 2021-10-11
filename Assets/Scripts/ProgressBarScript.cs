using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBarScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform endLine;
    [SerializeField] Slider slider;

    float maxDistance;

    [SerializeField] Text text1, text2;
    int level, lv1, lvl2;
    void Start()
    {
        level = SceneManager.GetActiveScene().buildIndex;
        lv1 = level + 1;
        text1.text = lv1.ToString();
        lvl2 = lv1 + 1;
        text2.text = lvl2.ToString();
        maxDistance = getDistance();
    }

    void Update()
    {
        if (player.position.z <= maxDistance && player.position.z <= endLine.position.z)
        {
            float distance = 1 - (getDistance() / maxDistance);
            setProgress(distance);
        }
    }

    float getDistance()
    {
        return Vector3.Distance(player.position, endLine.position);
    }

    void setProgress(float p)
    {
        slider.value = p;
    }

}
