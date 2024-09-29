using UnityEngine;

public class CircularRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
// Start is called before the first frame update
    void Start()
    {
    
    }

// Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
