using UnityEngine;

public class BoardManager:MonoBehaviour
{
    public Canvas board; // 面板
    public void Start()
    {
        board.enabled = false;
    }

    public void CloseBoard()
    {
        board.enabled = false;
    }
    public void OpenBoard()
    {
        board.enabled = true;
    }
}