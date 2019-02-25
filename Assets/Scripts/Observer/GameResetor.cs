using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// In Observer pattern this class is the concrete Observer,
/// which wants to be notified when events happend on @PlayerController.
/// 
/// @PlayerController doesn't care who wants the knowledge of winning the game,
/// it just passes the event to anyone who is interested... just like in observer pattern
/// </summary>
public class GameResetor : MonoBehaviour
{
    private void Start()
    {
        PlayerController pc = FindObjectOfType<PlayerController>();

        // Add the callbacks 
        pc.OnPlayerFellOff += OnPlayerFellOff;
        pc.OnGameWon += OnGameWon;
    }

    private void OnDisable()
    {
        PlayerController pc = FindObjectOfType<PlayerController>();

        // Callbacks must be always removed
        if (pc != null)
        {
            pc.OnPlayerFellOff -= OnPlayerFellOff;
            pc.OnGameWon -= OnGameWon;
        }
    }

    private void OnGameWon()
    {
        RestartGame();
    }

    private void OnPlayerFellOff()
    {
        RestartGame();
    }

    /// <summary>
    /// Restarts the current game scene
    /// </summary>
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
