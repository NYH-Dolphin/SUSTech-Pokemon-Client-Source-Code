using UnityEngine;
using UnityEngine.SceneManagement;

public class UserCollider: MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Monster"))
        {
            SceneManager.LoadScene("Fight");
        }
    }
}