using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    private static bool key = false;
    [SerializeField]
    private GameObject getKeyUI;

    int sceneNumber;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && key && this.gameObject.CompareTag("Goal"))
        {
            key = false;
            sceneNumber = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(2);
        }

        if (collision.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Key"))
        {
            key = true;
            getKeyUI.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    public void Next()
    {
        SceneManager.LoadScene(sceneNumber+1);
    }
}
