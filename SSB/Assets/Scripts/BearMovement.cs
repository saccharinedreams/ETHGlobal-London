using UnityEngine;

public class BearMovementAndAnimation : MonoBehaviour
{
    public float speed = 5f; // Speed at which the bears move forward
    private Animator animator; // Reference to the Animator component

    private void Start()
    {
        // Get the Animator component attached to the bear
        animator = GetComponent<Animator>();
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        animator.SetBool("Run Forward", true);
    }

    void Update()
    {
        // Move the bear forward
        transform.position += transform.forward * speed * Time.deltaTime;

        // Trigger the "Run Forward" animation by setting the "IsRunning" boolean parameter
        // Make sure you have a boolean parameter named "IsRunning" in your Animator for this to work
        //animator.SetBool("IsRunning", true);
    }
}
