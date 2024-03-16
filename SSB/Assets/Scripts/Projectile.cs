using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bear"))
        {
            BearController bearController = other.gameObject.GetComponent<BearController>();
            if (bearController != null)
            {
                bearController.HitByProjectile();
            }
            //Destroy(gameObject); // Destroy the projectile after hitting the bear
        }
    }
}
