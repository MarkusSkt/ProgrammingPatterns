using UnityEngine;

public class MoveCommand : IRigidbodyCommand
{
    private const int MoveSpeed = 10;

    public void Execute(Rigidbody rb)
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        rb.AddForce(movement * MoveSpeed);
    }
}
