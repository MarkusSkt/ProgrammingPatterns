using UnityEngine;

/// <summary>
/// Interface for all commands with rigidbody parameter.
/// Probably should have just done ICommand class, with multiple methods for commands with
/// different parameters... but this works fine for the test
/// </summary>
public interface IRigidbodyCommand
{
    void Execute(Rigidbody rb);
}
