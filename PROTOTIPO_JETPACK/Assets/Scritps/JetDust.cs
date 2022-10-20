using UnityEngine;

public class JetDust : MonoBehaviour
{
    [Header("Settings")]
    public Transform _parent;
    public ParticleSystem particle;
    public LayerMask _layerMask;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1.0f, _layerMask);

        if (hit.collider != null)
        {
            // Position
            _parent.position = hit.point;

            // Rotate to same angle as ground
            _parent.up = hit.normal;

            if (!particle.isPlaying)
                particle.Play();
            else if (transform.position.y == hit.point.y)
                particle.Stop();
        }
        else
        {
            // If raycast not hitting (air beneath feet), position it far away
            //_parent.position = new Vector3(0f, 1000f, 0f);
            particle.Stop();
        }
    }

   
}
