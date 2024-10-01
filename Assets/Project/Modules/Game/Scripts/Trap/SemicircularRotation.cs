using UnityEngine;

public class SemicircularRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float stopTime = 1.0f; 
    private bool isClockwise = true;
    private float currentRotation = 0f;
    private float stopTimer = 0f;

   
    void Update()
    {
        if (isClockwise)
        {
            if (stopTimer <= 0f)
            {
                transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
                currentRotation += rotationSpeed * Time.deltaTime;

                if (currentRotation >= 90f)
                {
                    isClockwise = false;
                    stopTimer = stopTime;
                }
            }
            else
            {
                stopTimer -= Time.deltaTime;
            }
        }
        else
        {
            if (stopTimer <= 0f)
            {
                transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
                currentRotation -= rotationSpeed * Time.deltaTime;

                if (currentRotation <= -90f)
                {
                    isClockwise = true;
                    stopTimer = stopTime;
                }
            }
            else
            {
                stopTimer -= Time.deltaTime;
            }
        }
    }
}
