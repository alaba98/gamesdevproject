using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    AimStateManager aim;
    Quaternion targetRotation;
    [SerializeField] float rotationSpeed; // Speed of rotation towards aim position


    private void Start()
    {
        aim = GetComponentInParent<AimStateManager>();
    }
    void Update()

    {
        if (aim.IsAiming())
        {
            RotateTowardsAimPosition();
        }
    }

    void RotateTowardsAimPosition()
    {
        Vector3 direction = aim.aimPosition.position - transform.position;
        direction.y = 0f; // Ensure the rotation is only on the horizontal plane

        targetRotation = Quaternion.LookRotation(direction);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
}

