using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    private static bool key = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && key)
        {
            SceneManager.LoadScene(2);
        }

        if (collision.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Key"))
        {
            key = true;
            Destroy(this.gameObject);
        }
    }
}
