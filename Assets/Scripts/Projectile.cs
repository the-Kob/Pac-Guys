using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 2.0f;

    private void Update()
    {
        transform.position += -transform.right * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}

