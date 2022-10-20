using UnityEngine;

public class CharacterDead : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool falling;

    void Start()
    {
        falling = true;
        rb.AddForce(transform.position * 0.5f, ForceMode2D.Impulse);
        MyCameraManger.Instance.FollowPlayer(this.gameObject);
        MyCameraManger.Instance.CameraZoonOut();
    }

    private void Update()
    {
        if (rb.velocity.sqrMagnitude < 0.01f && rb.angularVelocity < .01f && falling)
        {
            // Chama método para gameover
            falling = false;

            if (LevelManager.instance != null)
                LevelManager.instance.OnFailed();
        }
    }
}
