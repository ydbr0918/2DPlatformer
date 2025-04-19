using UnityEngine;
using UnityEngine.SceneManagement;

public class BossTriggerZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 보스랑 부딪힘 - 리스폰");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}