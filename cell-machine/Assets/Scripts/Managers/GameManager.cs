using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState state;
    public static event Action<GameState> onGameStateChanged;

    public enum GameState
    {
        STARTMENU,
        START,
        FINISH
    }
    public void UpdateGameState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.STARTMENU:
                {
                    break;
                }
            case GameState.START:
                {
                    break;
                }
            case GameState.FINISH:
                {
                    break;
                }
            default:
                print("No State");
                break;
        }

        onGameStateChanged?.Invoke(newState);
    }
}
