using UnityEngine;

public class LookAt : MonoBehaviour
{   
    public Transform target;
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {   
        bool hit1 = animator.GetBool("hit1");
        if(target != null)
        {
            Vector3 direction = target.position - transform.position;
            // Menghitung arah pandang tanpa memperhitungkan rotasi pada sumbu X dan Z
            Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = rotation;
        }
    }
}