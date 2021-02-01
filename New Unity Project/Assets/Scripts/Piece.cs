using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    Block[] blocks;
    public Vector2Int point;
    int[,] blockArray;
    PieceType type;
    public PieceType Type { get { return type; } }
    bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }
    int rotateCount;


    void Awake()
    {
        blocks = transform.GetComponentsInChildren<Block>();
        isGrounded = false;
        rotateCount = 0;
    }
    public void Init(PieceType type)
    {
        this.type = type;
        blockArray = GetInitialArray(type);
    }
    void OnTriggerEnter(Collider collider)
    {
        isGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
    public void HardDrop(MapManager mapManager)
    {
        int length = blockArray.GetLength(0);
        int y = 0;
        bool isAtGround = false;
        while(!isAtGround)
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (blockArray[j, i] == 0)
                        continue;
                    Vector2Int p = point + new Vector2Int(-length / 2 + i, -(-length / 2 + j)) - new Vector2Int(0, y+1);
                    if (!mapManager.IsInsideMap(p))
                        isAtGround = true;
                    else if (mapManager.ValueAt(p) == 1)
                        isAtGround = true;
                }
            }
            y++;
        }
        point.y -= y - 1;
        transform.position = mapManager.PosOf(point);
    }
    public bool CanMove(int xDir, MapManager mapManager)
    {
        int length = blockArray.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (blockArray[j, i] == 0)
                    continue;
                Vector2Int p = point + new Vector2Int(-length/2 + i,-(-length/2 + j)) + new Vector2Int(xDir, 0);
                if (!mapManager.IsInsideMap(p))
                    return false;
                if (mapManager.ValueAt(p) == 1)
                    return false;
            }
        }
        return true;
    }

    public void Move(int xDir)
    {
        transform.position += new Vector3(xDir, 0, 0) * Block.size;
        point.x += xDir;
    }

    //Returns null if it can't rotate
    public KeyValuePair<bool, Vector2Int> GetRotationOffset(bool clockwise, MapManager mapManager)
    {
        Vector2Int[,] offsetTable = GetOffsetTable(type);
        int c = clockwise ? 1 : -1;
        for (int i=0; i<offsetTable.GetLength(1);i++)
        {
            Vector2Int offset = offsetTable[rotateCount, i] - offsetTable[(rotateCount + c + 4)%4, i];
            if (CheckRotation(clockwise, offset, mapManager))
            {
                return new KeyValuePair<bool, Vector2Int>(true, offset);
            }
        }
        return new KeyValuePair<bool, Vector2Int>(false, new Vector2Int());
    }

    public void SetPoint(Vector2Int vector2Int)
    {
        point = vector2Int;
    }

    public void Rotate(bool clockwise, Vector2Int offset)
    {
        if (clockwise)
            rotateCount += 1;
        else
            rotateCount -= 1;
        rotateCount = (rotateCount + 4) % 4;
        transform.rotation = Quaternion.Euler(0, 0, rotateCount * -90);
        blockArray = GetRotatedArray(clockwise);
        transform.position += new Vector3(offset.x, offset.y, 0) * Block.size;
        point += offset;
    }

    public void ReturnBlocks(MapManager mapManager)
    {
        int length = blockArray.GetLength(0);
        int c = 0;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (blockArray[j,i] == 0)
                    continue;
                Vector2Int p = point + new Vector2Int(-length / 2 + i, -(-length / 2 + j));
                mapManager.AddBlock(blocks[c++], p);
                Debug.Log(p);
            }
        }
        Destroy(gameObject);
    }

    public int[,] GetInitialArray(PieceType type)
    {
        int[,] tempArray;
        switch (type)
        {
            case PieceType.I:
                tempArray = new int[5, 5] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 1, 1, 1, 1 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
                break;
            case PieceType.T:
                tempArray = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
                break;
            case PieceType.J:
                tempArray = new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
                break;
            case PieceType.L:
                tempArray = new int[3, 3] { { 0, 0, 1 }, { 1, 1, 1 }, { 0, 0, 0 } };
                break;
            case PieceType.S:
                tempArray = new int[3, 3] { { 0, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 }};
                break;
            case PieceType.Z:
                tempArray = new int[3, 3] { { 1, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
                break;
            case PieceType.O:
            default:
                tempArray = new int[3, 3] { { 0, 1, 1 }, { 0, 1, 1 }, { 0, 0, 0 } };
                break;
        }
        return tempArray;
    }
    bool CheckRotation(bool clockwise, Vector2Int offset, MapManager mapManager)
    {
        int [,]tempArray = GetRotatedArray(clockwise);
        int length = tempArray.GetLength(0);
        for(int i=0; i<length; i++)
        {
            for(int j=0; j<length; j++)
            {
                if (tempArray[j, i] == 1)
                {
                    Vector2Int t = new Vector2Int(point.x -length/2 + i + offset.x, point.y +length/2- j + offset.y);
                    if (!mapManager.IsInsideMap(t))
                        return false;
                    if(mapManager.ValueAt(t) == 1)
                        return false;
                }
            }
        }
        return true;
    }
    public int[,] GetRotatedArray(bool clockwise)
    {
        int[,] result = (int[,])blockArray.Clone();
        int length = blockArray.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (clockwise)
                    result[i, length - j - 1] = blockArray[j, i];
                else
                    result[length - 1 - i, j] = blockArray[j, i];
            }
        }
        return result;
    }

    public Vector2Int[,] GetOffsetTable(PieceType type)
    {
        if (type == PieceType.I)
        {
            return new Vector2Int[4, 5]
            {
                { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(+2,0), new Vector2Int(-1,0), new Vector2Int(+2,0) },
                { new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,+1), new Vector2Int(0,-2) },
                { new Vector2Int(-1,+1), new Vector2Int(+1,+1), new Vector2Int(-2,+1), new Vector2Int(+1,0), new Vector2Int(-2,0) },
                { new Vector2Int(0,+1), new Vector2Int(0,+1), new Vector2Int(0,+1), new Vector2Int(0,-1), new Vector2Int(0,+2) }
            };
        }
        else if (type == PieceType.O)
        {
            return new Vector2Int[4,1]
            {
                { new Vector2Int(0,0) },
                { new Vector2Int(0,-1) },
                { new Vector2Int(-1,-1) },
                { new Vector2Int(-1,0) }
            };
        }
        else
        {
            return new Vector2Int[4, 5]
            {
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(+1,0), new Vector2Int(+1,-1), new Vector2Int(0,+2), new Vector2Int(+1,+2) },
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,-1), new Vector2Int(0,+2), new Vector2Int(-1,+2) }
            };
        }
    }
}
