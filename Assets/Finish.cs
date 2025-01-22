using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject VictoryMenu;

    public void Awake()
    {
        VictoryMenu.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            VictoryMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
