using UnityEngine;

public class PickupScore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.Instance.AddScore(10);
            gameObject.SetActive(false);
        }
    }
}
