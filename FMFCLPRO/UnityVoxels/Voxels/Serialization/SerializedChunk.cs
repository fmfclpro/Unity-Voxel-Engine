using FMFCLPRO.UnityVoxels.Utils;
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

namespace FMFCLPRO.Voxels.Serialization
{
    [CreateAssetMenu(menuName = "Create SerializedChunk", fileName = "SerializedChunk", order = 0)]
    public class SerializedChunk : ScriptableObject
    {
        public ushort[] chunkBlocks;
        public int bytes;

        public ushort GetChunkData(int x, int y, int z, int w, int d)
        {
            return chunkBlocks[x + w * (y + z * z)];
        }

        public void SetChunkData(int x, int y, int z, int w, int d, ushort id)
        {
            chunkBlocks[x + w * (y + z * z)] = id;
        }

        public ushort[,,] ToGrid(int gridSizex, int gridSizeY, int gridSizeZ)
        {
            return ArrayUtils.ArrayTo3DArray(chunkBlocks, gridSizex, gridSizeY, gridSizeZ);
        }

        public void SerializeChunk(ushort[,,] chunkData)
        {
            bytes = 0;
            chunkBlocks = new ushort[chunkData.GetLength(0) * chunkData.GetLength(1) * chunkData.GetLength(1)];
            for (int x = 0; x < chunkData.GetLength(0); x++)
            {
                for (int y = 0; y < chunkData.GetLength(1); y++)
                {
                    for (int z = 0; z < chunkData.GetLength(2); z++)
                    {
                        chunkBlocks[x + chunkData.GetLength(0) * (y + chunkData.GetLength(2) * z)] = chunkData[x, y, z];
                        bytes += sizeof(ushort);
                    }
                }
            }
        }
    }
}