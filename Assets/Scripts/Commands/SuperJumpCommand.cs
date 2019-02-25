using UnityEngine;

/// <summary>
/// Command for super jumps
/// </summary>
public class SuperJumpCommand : IRigidbodyCommand
{
    private readonly Vector3 SuperJumpForce = new Vector3(0, 1000, 0);

    public void Execute(Rigidbody rb)
    {
        rb.AddForce(SuperJumpForce);
    }
}
