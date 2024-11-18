using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    public bool key = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        

        
        if (collision.gameObject.CompareTag("Player") && key)
        {
            SceneManager.LoadScene(2);
        }

        if (collision.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Key"))
        {
            Destroy(this.gameObject);
        }
    }
}
