using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10;
    public Vector3 direction;
    private float initialPosition;
    public Transform midAirExplosionPrefab;
    public float maxDistance = 10;
    public int damagePoints = 25;

    private void Awake()
    {
        direction = transform.right;
        initialPosition = transform.position.x;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
    }

    private void FixedUpdate()
    {
        if(Mathf.Abs(transform.position.x - initialPosition) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Player")
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damagePoints);
            else if(collision.gameObject.tag == "Enemy")
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(damagePoints);
            Instantiate(midAirExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
