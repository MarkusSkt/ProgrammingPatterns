using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles registering/executing commands.
/// This class is not fundamental to the pattern, but makes
/// it easier to handle commands
/// </summary>
public class CommandFactory
{
    private Dictionary<string, IRigidbodyCommand> commands = new Dictionary<string, IRigidbodyCommand>();

    /// <summary>
    /// Registers a new command to the @commands list
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="commandKey"></param>
    public void RegisterRbCommand<TCommand>(string commandKey) where TCommand : IRigidbodyCommand, new()
    {
        // Instantiate the command
        IRigidbodyCommand command = new TCommand();

        commands.Add(commandKey, command);
    }

    /// <summary>
    /// Tries to execute a command with @commandKey
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="commandKey"></param>
    public void ExecuteRbCommand(string commandKey, Rigidbody rb)
    {
        IRigidbodyCommand command;
        if (!commands.TryGetValue(commandKey, out command))
        {
            Debug.LogWarning("Command: " + commandKey + " not found!");
            return;
        }

        command.Execute(rb);
    }
}
