using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    Block[] blocks;
    Vector2Int[] blockPoints;
    int[,] blockArray;
    int rotateCount;
    PieceType type;
    public PieceType Type { get { return type; } }
    bool isGrounded;
    public bool IsGrounded {  get { return isGrounded; } }

    void Awake()
    {
        blocks = transform.GetComponentsInChildren<Block>();
        blockPoints = new Vector2Int[4];
        rotateCount = 0;
        isGrounded = false;
    }
    void OnTriggerEnter(Collider collider)
    {
        isGrounded = true;
    }
    public void InitBlockPoints(MapManager mapManager)
    {
        for(int i=0; i<4; i++)
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

        int[,] originalArray = blockArray;
        int length = blockArray.GetLength(0);
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < length; x++)
            {
                if (clockwise)
                    blockArray[x, length - y - 1] = originalArray[y, x];
                else
                    blockArray[length - x - 1, y] = originalArray[y, x];
            }
        }
    }
    public void Move(int xDir)
    {
        transform.position += new Vector3(xDir, 0, 0) * Block.size;
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
