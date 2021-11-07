using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Canvas board; // 面板

    public virtual void Start()
    {
        board.enabled = false;
    }

    public virtual void CloseBoard()
    {
        board.enabled = false;
    }

    public virtual void OpenBoard()
    {
        board.enabled = true;
    }
}