using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layerDeath = LayerMask.NameToLayer("Death");

        if (collision.gameObject.CompareTag("Death"))
        {
            PlayerDeathHandler();
            RestartLevel();
        }
    }    
    private void PlayerDeathHandler()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().name));
    }
}
