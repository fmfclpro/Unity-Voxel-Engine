using System;
using FMFCLPRO.UnityVoxels.Voxels.Core;
using UnityEngine;

/*
MIT License

Copyright (c) 2023 Filipe Lopes | FMFCLPRO

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/


namespace FMFCLPRO.Voxels.Core
{
    [Serializable]
public struct VoxelPosition : IEquatable<VoxelPosition>
{
    private int _x;
    private int _y;
    private int _z;
    public VoxelPosition(int x, int y, int z)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    public override string ToString()
    {
        return $"X: {_x} || Y: {_y} || Z: {_z}";
    }

    public void PrintPosition()
    {
        Debug.Log(ToString());
    }

    public static implicit operator VoxelPosition(Vector3Int vector3Int)
    {
        return new VoxelPosition(vector3Int.x, vector3Int.y, vector3Int.z);
    }
    public static implicit operator VoxelPosition(Vector3 vector3)
    {
        int d0 = (int) (Mathf.Round(vector3.x));
        int d1 = (int) (Mathf.Round(vector3.y));
        int d2 = (int) (Mathf.Round(vector3.z));
        return new VoxelPosition(d0, d1, d2);
    }
    public  VoxelPosition Forward => new VoxelPosition(_x, _y, _z +1);
    public  VoxelPosition Back => new VoxelPosition(_x, _y, _z -1);
    public  VoxelPosition Up => new VoxelPosition(_x, _y + 1,_z);
    public  VoxelPosition Down => new VoxelPosition(_x, _y -1 , _z);
    public  VoxelPosition Right => new VoxelPosition(_x + 1    , _y, _z);
    public  VoxelPosition Left => new VoxelPosition(_x -1, _y    , _z);

    public VoxelPosition GetDirection(VoxelSide side)
    {
        switch (side)
        {
            case VoxelSide.Forward:
                return Forward;
            case VoxelSide.Back:
                return Back;
            case VoxelSide.Down:
                return Down;
            case VoxelSide.Up:
                return Up;
            case VoxelSide.Left:
                return Left;
            case VoxelSide.Right:
                return Right;
            default:
                throw new NotImplementedException();
        }
    }
    public int X => _x;
    public int Y => _y;
    public int Z => _z;

    public bool Equals(VoxelPosition other)
    {
        return _x == other._x && _y == other._y && _z == other._z;
    }

    public override bool Equals(object obj)
    {
        return obj is VoxelPosition other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_x, _y, _z);
    }
}
}