using UnityEngine;
using System.Collections;

public class EnemyCollision : MonoBehaviour
{
    public GameObject bossPrefab;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enemy Collision");
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;
        Instantiate(bossPrefab, position, rotation);
        Destroy(gameObject);
        Debug.Log("Enemy Destroyed");
    }
}