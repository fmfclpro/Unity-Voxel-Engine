using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FMFCLPRO.Registry;
using FMFCLPRO.UnityVoxels.Voxels.Atlas;
using FMFCLPRO.UnityVoxels.Voxels.Core.BlockDatas;
using FMFCLPRO.UnityVoxels.Voxels.Core.Blocks;
using FMFCLPRO.UnityVoxels.Voxels.Shapes;
using FMFCLPRO.Voxels.Jobs;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;

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

namespace FMFCLPRO.UnityVoxels.Voxels.Core.World
{
    public class Chunk : MonoBehaviour
    {
        public Material atlas;
        public int width;
        public int height;
        public int depth;
        public TextureAtlas _textureAtlas;
        public Vector3 location;
        public WorldChunking worldChunking;
        private BlockInstance[,,] chunkData;

        //public BlockInstance[,,] BlockInstances;
        private MeshFilter mf;
        private MeshRenderer mr;
        private MeshCollider collider;

        private bool isSetToUpdate;

        //Flat[x + WIDTH * (y + DEPTH * z)] = Original[x, y, z]
        //x = i % WIDTH
        //y = (i / WIDTH) % HEIGHT
        //z = i / (WIDTH * HEIGHT )

        CalculateBlockTypes calculateBlockTypes;
        JobHandle jobHandle;

        public BaseBlock GetVoxelData(int x, int y, int z)
        {
            //ushort id = chunkData[x, y, z];
            ushort id = chunkData[x, y, z].ID;
            BaseBlock data = RegisteredBlocks.BlockDatas[id];
            return data;
        }

        public void SetVoxel(Vector3Int position, BaseBlock baseBlock)
        {
            SetVoxel(position.x, position.y, position.z, baseBlock);
        }

        public bool SetVoxel(int x, int y, int z, BaseBlock baseBlock)
        {
            if (!InBounds(x, y, z)) return false;
            chunkData[x, y, z] = baseBlock.GetInstance();
            isSetToUpdate = true;
            return true;
        }

        public bool SetVoxel(int x, int y, int z, BaseBlock baseBlock, bool refreh)
        {
            if (!InBounds(x, y, z)) return false;
            chunkData[x, y, z] = baseBlock.GetInstance();
            isSetToUpdate = refreh;
            return true;
        }

        private bool InBounds(int x, int y, int z)
        {
            return InBoundsX(x) && InBoundsY(y) && InBoundsZ(z);
        }

        private bool InBoundsX(int x)
        {
            return x >= 0 && x <= width - 1;
        }

        private bool InBoundsY(int y)
        {
            return y >= 0 && y <= height - 1;
        }

        private bool InBoundsZ(int z)
        {
            return z >= 0 && z <= depth - 1;
        }

        private bool InBounds(Vector3Int pos)
        {
            return InBounds(pos.x, pos.y, pos.z);
        }

        public void SetForRefresh()
        {
            isSetToUpdate = true;
        }

        public IEnumerator Refresh()
        {
            RefreshChunk();
            yield return null;
            c = null;
        }


        public void AllocateChunk(Vector3 dimensions, Vector3 position)
        {
            location = position;
            width = (int)dimensions.x;
            height = (int)dimensions.y;
            depth = (int)dimensions.z;

            mf = gameObject.AddComponent<MeshFilter>();
            mr = gameObject.AddComponent<MeshRenderer>();
            mr.material = atlas;

            AllocateChunkData(width, height, depth);
        }

        public void InitializeChunk()
        {
            collider = this.gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mf.mesh;
        }


        private void AllocateChunkData(int width, int height, int depth)
        {
            //  chunkData = new ushort[width, height, depth];
            chunkData = new BlockInstance[width, height, depth];
            blocks = new IVoxelShape[width, height, depth];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < depth; k++)
                    {
                        SetVoxel(i, j, k, RegisteredBlocks.Air);
                    }
                }
            }
        }

        public void SetChunkData(ushort[,,] data)
        {
            //chunkData = data;
        }

        private Stopwatch _stopwatch = new Stopwatch();

        private IVoxelShape[,,] blocks;

        public void RefreshChunk()
        {
            // _stopwatch.Start();

            var inputMeshes = new List<Mesh>();
            int vertexStart = 0;
            int triStart = 0;
            int meshCount = width * height * depth;
            int m = 0;
            var jobs = new ProcessMeshDataJob
            {
                VertexStart =
                    new NativeArray<int>(meshCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory),
                TriStart = new NativeArray<int>(meshCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory)
            };


            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        ushort k = chunkData[x, y, z].ID;
                        BaseBlock baseBlock = RegisteredBlocks.BlockDatas[k];
                        IVoxelShape voxelShape = baseBlock.RenderShape();

                        blocks[x, y, z] = voxelShape;
                        blocks[x, y, z].Render(worldChunking, new Vector3(x, y, z) + location, baseBlock, this,
                            _textureAtlas);

                        if (blocks[x, y, z].Mesh != null)
                        {
                            inputMeshes.Add(blocks[x, y, z].Mesh);
                            var vcount = blocks[x, y, z].Mesh.vertexCount;
                            var icount = (int)blocks[x, y, z].Mesh.GetIndexCount(0);
                            jobs.VertexStart[m] = vertexStart;
                            jobs.TriStart[m] = triStart;
                            vertexStart += vcount;
                            triStart += icount;
                            m++;
                        }
                    }
                }
            }


            jobs.MeshData = Mesh.AcquireReadOnlyMeshData(inputMeshes);
            var outputMeshData = Mesh.AllocateWritableMeshData(1);
            jobs.OutputMesh = outputMeshData[0];
            jobs.OutputMesh.SetIndexBufferParams(triStart, IndexFormat.UInt32);
            jobs.OutputMesh.SetVertexBufferParams(vertexStart,
                new VertexAttributeDescriptor(VertexAttribute.Position),
                new VertexAttributeDescriptor(VertexAttribute.Normal, stream: 1),
                new VertexAttributeDescriptor(VertexAttribute.TexCoord0, stream: 2));


            var handle = jobs.Schedule(inputMeshes.Count, 4);
            var newMesh = new Mesh();
            var sm = new SubMeshDescriptor(0, triStart, MeshTopology.Triangles);
            sm.firstVertex = 0;
            sm.vertexCount = vertexStart;


            handle.Complete();


            jobs.OutputMesh.subMeshCount = 1;
            jobs.OutputMesh.SetSubMesh(0, sm);
            Mesh.ApplyAndDisposeWritableMeshData(outputMeshData, new[] { newMesh });

            jobs.MeshData.Dispose();
            jobs.VertexStart.Dispose();
            jobs.TriStart.Dispose();
            newMesh.RecalculateBounds();

            mf.mesh = newMesh;

            collider.sharedMesh = mf.mesh;
            // _stopwatch.Stop();
            //
            // string message = $"Chunk at: {location} has been refreshed. Time took {_stopwatch.ElapsedMilliseconds}";
            // _stopwatch.Reset();
        }

        private Coroutine c;

        private void LateUpdate()
        {
            if (isSetToUpdate)
            {
                RefreshChunk();
                isSetToUpdate = false;
            }
        }
    }
}