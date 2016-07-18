using UnityEngine;


public class Rotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
    }
}

