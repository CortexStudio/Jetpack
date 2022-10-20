using UnityEngine;
using UnityEngine.UI;

public class AngleCalculator : MonoBehaviour
{
    private Vector3 lastFwd;
    private float curAngleZ = 0;


    void Start()
    {
        lastFwd = transform.up;
    }

    void Update()
    {

        Vector3 curFwd = transform.up;
        // measure the angle rotated since last frame:
        float ang = Vector3.Angle(curFwd, lastFwd);
        if (ang > 0.01)
        {
            // if rotated a significant angle...
            // fix angle sign...
            if (Vector3.Cross(curFwd, lastFwd).z < 0) ang = -ang;

            curAngleZ += ang; // accumulate in curAngleZ...
            lastFwd = curFwd; // and update lastFwd

            if (curAngleZ <= -290) //if (curAngleZ < -290 || curAngleZ > 290)
            {
                curAngleZ = 20;

                if (LevelManager.instance != null)
                    LevelManager.instance.Active360();
            }
            else if (curAngleZ >= 290)
            {
                curAngleZ = -20;

                if (LevelManager.instance != null)
                    LevelManager.instance.Active360();
            }
        }
    }
}
