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
}
