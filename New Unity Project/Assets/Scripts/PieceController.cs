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
            currentPiece.Rotate(true);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            currentPiece.Rotate(false);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentPiece.Move(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentPiece.Move(1);
        }
        if (currentPiece.IsGrounded)
        {
            CreatePiece();
        }
        else
            currentPiece.transform.position += new Vector3(0, -1, 0) * dropSpeed * Time.deltaTime;
    }
    public void CreatePiece()
    {
        Vector3 spawnPosition = mapManager.PosOf(spawnPoint.x, spawnPoint.y);   //spawnPoint : blockarray[1][1]
        GameObject t = Instantiate(piecePrefabs[nextPiece], spawnPosition, Quaternion.identity,transform);
        currentPiece = t.GetComponent<Piece>();
        if (nextPiece == (int)PieceType.I || nextPiece == (int)PieceType.O)
            t.transform.position += new Vector3(-0.5f, 0.5f, 0) * Block.size;
        nextPiece = (nextPiece + 1) % 7;
    }
    public bool IsGrounded()
    {
        return false;
    }
}
