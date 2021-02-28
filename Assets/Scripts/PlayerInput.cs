using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInput : MonoBehaviour
{
    private static KeyCode[] DROP_HARD = { KeyCode.Space, KeyCode.W, KeyCode.UpArrow };
    private static KeyCode[] DROP_FAST = { KeyCode.S, KeyCode.DownArrow };
    private static KeyCode[] ROTATE_LEFT = { KeyCode.E };
    private static KeyCode[] ROTATE_RIGHT = { KeyCode.Q };
    private static KeyCode[] MOVE_LEFT = {KeyCode.A, KeyCode.LeftArrow};
    private static KeyCode[] MOVE_RIGHT = {KeyCode.D, KeyCode.RightArrow};

    [SerializeField]
    private BoardController _boardController;

    // Update is called once per frame
    private void Update()
    {
        _boardController.DropFast = DROP_FAST.Any(k => Input.GetKey(k));

        if (DROP_HARD.Any(k => Input.GetKeyDown(k)))
        {
            _boardController.DropHard();
        }

        if (ROTATE_LEFT.Any(k => Input.GetKeyDown(k)))
        {
            _boardController.RotateLeft();
        }

        if (ROTATE_RIGHT.Any(k => Input.GetKeyDown(k)))
        {
            _boardController.RotateRight();
        }

        if(MOVE_LEFT.Any(k => Input.GetKeyDown(k)))
        {
            _boardController.MoveLeft();
        }

        if(MOVE_RIGHT.Any(k => Input.GetKeyDown(k)))
        {
            _boardController.MoveRight();
        }
    }
}
