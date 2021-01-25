using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    Block[] blocks;
    public Vector2Int[] blockPoints;
    int[,] blockArray;
    PieceType type;
    public PieceType Type { get { return type; } }
    bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }
    int rotateCount;

    void Awake()
    {
        blocks = transform.GetComponentsInChildren<Block>();
        blockPoints = new Vector2Int[4];
        isGrounded = false;
        rotateCount = 0;
    }
    void OnTriggerEnter(Collider collider)
    {
        isGrounded = true;
    }
    public bool CanMove(int xDir, MapManager mapManager)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            Vector2Int point = blockPoints[i] + new Vector2Int(xDir, 0);
            if (point.x < 0 || point.x >= MapManager.width || mapManager.ValueAt(point) == 1)
                return false;
        }
        return true;
    }

    public void Move(int xDir)
    {
        transform.position += new Vector3(xDir, 0, 0) * Block.size;
        for (int i = 0; i < blocks.Length; i++)
            blockPoints[i].x += xDir;
    }

    public void UpdatePoints(MapManager mapManager)
    {
        for (int i = 0; i < blocks.Length; i++)
            blockPoints[i] = mapManager.PosToPoint(blocks[i].transform.position);
    }

    public void Rotate(bool clockwise)
    {
        if (clockwise)
            rotateCount += 1;
        else
            rotateCount -= 1;
        rotateCount = (rotateCount + 4) % 4;
        transform.rotation = Quaternion.Euler(0, 0, rotateCount * -90);
    }

    public void ReturnBlocks(MapManager mapManager)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            mapManager.AddBlock(blocks[i],blockPoints[i]);
        }
        Destroy(gameObject);
    }

    public int[,] GetInitialArray(PieceType type)
    {
        int[,] blockArray;
        switch (type)
        {
            case PieceType.I:
                blockArray = new int[4, 4] { { 0, 0, 0, 0 }, { 1, 1, 1, 1 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
                break;
            case PieceType.T:
                blockArray = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
                break;
            case PieceType.J:
                blockArray = new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
                break;
            case PieceType.L:
                blockArray = new int[3, 3] { { 0, 0, 1 }, { 1, 1, 1 }, { 0, 0, 0 } };
                break;
            case PieceType.S:
                blockArray = new int[3, 3] { { 0, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 }};
                break;
            case PieceType.Z:
                blockArray = new int[3, 3] { { 1, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
                break;
            case PieceType.O:
            default:
                blockArray = new int[2, 2] { { 1, 1 }, { 1, 1 } };
                break;
        }
        return blockArray;
    }
}
