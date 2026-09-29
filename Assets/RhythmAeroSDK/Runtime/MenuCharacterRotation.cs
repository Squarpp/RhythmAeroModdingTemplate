using UnityEngine;

public class MenuCharacterRotation : MonoBehaviour
{
    [SerializeField] public bool clockwise = true;
    [SerializeField] private float rotationSpeed = 45f; // Grados por segundo

    [SerializeField] private bool rotateX = false;
    [SerializeField] private bool rotateY = false;
    [SerializeField] private bool rotateZ = false;

    public float speed = 1;

    void Update()
    {
        float direction = clockwise ? 1f : -1f;
        float actualSpeed = rotationSpeed * speed;

        if (rotateY)
        {
            transform.Rotate(0f, direction * actualSpeed * Time.deltaTime, 0f, Space.Self);
        }
        else if (rotateZ)
        {
            transform.Rotate(0f, 0f, direction * actualSpeed * Time.deltaTime, Space.Self);
        }else if(rotateX)
            {   
            transform.Rotate(direction * actualSpeed * Time.deltaTime, 0f, 0f,Space.Self); 
            }
    }
}