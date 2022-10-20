using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class CharacterControl : MonoBehaviour
{

    private Rigidbody2D[] rbRagdoll;


    private void Awake()
    {
        rbRagdoll = GetComponentsInChildren<Rigidbody2D>();
        DisableRagdoll();
    }


    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisableRagdoll()
    {
        foreach (var rbs in rbRagdoll)
        {
            rbs.isKinematic = true;
        }
    }
}
