using UnityEngine;
using System.Collections;


public class MoveObject : MonoBehaviour
{
    [Tooltip("small object will move slower that biger(planets)")]
    public bool speedDependsOnScale;


    private Vector3 offset;
    public float speed = -0.2f;


    void Start()
    {
        offset = new Vector3(0, 0, speed);

    }


    void Update()
    {
        transform.position += offset * Time.deltaTime;                             // set new position
    }

}  // Karol Sobanski


