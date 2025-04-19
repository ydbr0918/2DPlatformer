using UnityEngine;
using UnityEngine.SceneManagement;

public class BossTriggerZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾ ������ �ε��� - ������");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}