using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    private static bool key = false;
    [SerializeField]
    private GameObject getKeyUI;
    public AudioClip getKeySE;
    public AudioClip goalSE;
    private static AudioSource audioSource;
    static int sceneNumber;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && key && this.gameObject.CompareTag("Goal"))
        {
            audioSource.PlayOneShot(goalSE);
            key = false;
            sceneNumber = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine("Goal");
        }

        if (collision.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Key"))
        {
            key = true;
            audioSource.PlayOneShot(getKeySE);
            getKeyUI.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    IEnumerator Goal()
    {
        yield return new WaitForSeconds(0.1f);
        if (sceneNumber == 4)
        {
            SceneManager.LoadScene(8);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }
    public void Next()
    {
        SceneManager.LoadScene(sceneNumber+1);
    }
}
