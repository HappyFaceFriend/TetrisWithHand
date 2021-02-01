using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PieceController pieceController;
    public MapManager mapManager;

    Coroutine gameCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        gameCoroutine = StartCoroutine(GameCoroutine());
    }

    IEnumerator GameCoroutine()
    {
        pieceController.CreatePiece();
        while(true)
        {
            pieceController.UpdateFrame();
            yield return null;
        }
    }
}
