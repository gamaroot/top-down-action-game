using UnityEngine;

public class SpringPusher : MonoBehaviour
{
    public float bounceForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 bounceDirection = new Vector3(0, 0, Mathf.Sign(collision.transform.position.z - transform.position.z));
            rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
    }
}
