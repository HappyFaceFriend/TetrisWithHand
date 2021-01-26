using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public Vector2Int spawnPoint;
    Piece currentPiece;
    public float dropSpeed =1f;
    public GameObject[] piecePrefabs;
    public MapManager mapManager;
    public int nextPiece = 1;
    int rotateCount;

    // Start is called before the first frame update
    void Start()
    {
        CreatePiece();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            var val = currentPiece.GetRotationOffset(true, mapManager);
            if(val.Key)
                currentPiece.Rotate(true, val.Value);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            var val = currentPiece.GetRotationOffset(false, mapManager);
            if (val.Key)
                currentPiece.Rotate(false, val.Value);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(currentPiece.CanMove(-1, mapManager))
                currentPiece.Move(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentPiece.CanMove(1, mapManager))
                currentPiece.Move(1);
        }
        if (currentPiece.IsGrounded)
        {
            currentPiece.ReturnBlocks(mapManager);
            CreatePiece();
        }
        else
        {
            currentPiece.transform.position += new Vector3(0, -1, 0) * dropSpeed * Time.deltaTime;
            currentPiece.SetPoint(mapManager.PosToPoint(currentPiece.transform.position));
        }
    }
    public void CreatePiece()
    {
        Vector3 spawnPosition = mapManager.PosOf(spawnPoint.x, spawnPoint.y);
        GameObject t = Instantiate(piecePrefabs[nextPiece], spawnPosition, Quaternion.identity,transform);
        currentPiece = t.GetComponent<Piece>();
        currentPiece.Init((PieceType)nextPiece);
        nextPiece = (nextPiece + 1) % 7;
        rotateCount = 0;
    }

}
