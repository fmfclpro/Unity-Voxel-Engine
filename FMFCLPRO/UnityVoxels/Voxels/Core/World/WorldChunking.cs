using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FMFCLPRO.Registry;
using FMFCLPRO.UnityVoxels.Voxels.Atlas;
using FMFCLPRO.UnityVoxels.Voxels.Core.Blocks;
using FMFCLPRO.Voxels.Core;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
    public class WorldChunking : MonoBehaviour
    {
        public Vector3Int worldDimensions;
        public Vector3Int chunkDimensions;
        public GameObject chunkPrefab;
        private Chunk[,,] _chunks;

        private Dictionary<Vector3Int, Chunk> dicChunks = new Dictionary<Vector3Int, Chunk>();
        public Texture2D[] Texture2Ds;
        private TextureAtlas _textureAtlas;


        public MeshFilter objectKappa;


        public Mesh GetCustomMesh()
        {
            return objectKappa.mesh;
        }

        void Start()
        {
            Application.targetFrameRate = 60;
            _textureAtlas = AtlasUtils.StitchTextures(Texture2Ds);

            StartCoroutine(Createworld());
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < worldDimensions.x; i++)
            {
                for (int j = 0; j < worldDimensions.z; j++)
                {
                    for (int k = 0; k < worldDimensions.y; k++)
                    {
                        Vector3 pos = new Vector3(i * 5, k * 5, j * 5);
                        pos += new Vector3(2f, 2f, 2f);
                        Gizmos.DrawWireCube(pos, new Vector3(5, 5, 5));
                    }
                }
            }
        }


        private float kappa;

        private void Update()
        {
            kappa += Time.deltaTime;
            if (kappa <= 0.1f) return;


            Camera camera = Camera.main;

            Vector3 forward = Camera.main.transform.forward * 35;

            // if (n .GetKeyDown(KeyCode.Alpha5))
            // {
            //     VoxelPosition p0 = forward;
            //   
            //     
            //     for (int j = -5; j < 5; j++)
            //     {
            //         for (int k = -5; k < 5; k++)
            //         {
            //             for (int l = -5; l < 5; l++)
            //             {
            //                 float distance = Vector3.Distance(forward, new Vector3(
            //                     p0.X + j, p0.Y + k, p0.Z + l));
            //                 if (distance <= 4)
            //                 {
            //                     SetBlockInWorld(p0.X + j , p0.Y + k , p0.Z + l, RegisteredBlocks.Dirt);
            //                 }
            //             }
            //                 
            //         }
            //             
            //     }
            // }
            //

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                Chunk chunk = raycastHit.collider.gameObject.GetComponent<Chunk>();

                Vector3 hitBlock = raycastHit.point - raycastHit.normal / 2f;

                Vector3 blockBuld = raycastHit.point + raycastHit.normal / 2f;

                int d0 = (int)(Mathf.Round(hitBlock.x));
                int d1 = (int)(Mathf.Round(hitBlock.y));
                int d2 = (int)(Mathf.Round(hitBlock.z));

                int d3 = (int)(Mathf.Round(blockBuld.x));
                int d4 = (int)(Mathf.Round(blockBuld.y));
                int d5 = (int)(Mathf.Round(blockBuld.z));


                int x = (int)(Mathf.Round(hitBlock.x) - chunk.location.x);
                int y = (int)(Mathf.Round(hitBlock.y) - chunk.location.y);
                int z = (int)(Mathf.Round(hitBlock.z) - chunk.location.z);
                int i = x + chunkDimensions.x * (y + chunkDimensions.z * z);

                blockPos = new Vector3Int(x, y, z);


                if (Input.GetKey(KeyCode.Mouse1))
                {
                    SetBlockInWorld(d3, d4, d5, RegisteredBlocks.Dirt);
                    kappa = 0;
                }

                if (Input.GetKey(KeyCode.Alpha1))
                {
                    for (int j = -5; j < 5; j++)
                    {
                        for (int k = -5; k < 5; k++)
                        {
                            for (int l = -5; l < 5; l++)
                            {
                                float distance = Vector3.Distance(new Vector3(d0, d1, d2), new Vector3(
                                    d0 + j, d1 + k, d2 + l));
                                if (distance <= 4)
                                {
                                    SetBlockInWorld(d0 + j, d1 + k, d2 + l, RegisteredBlocks.Dirt);
                                }
                            }
                        }
                    }

                    kappa = 0;
                }

                if (Input.GetKey(KeyCode.Alpha2))
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        for (int k = -1; k <= 1; k++)
                        {
                            for (int l = -1; l <= 1; l++)
                            {
                                SetBlockInWorld(d0 + j, d1 + k, d2 + l, RegisteredBlocks.Air);
                            }
                        }
                    }

                    kappa = 0;
                }

                if (Input.GetKey(KeyCode.Alpha4))
                {
                    SetBlockInWorld(d3, d4, d5, RegisteredBlocks.Custom);


                    kappa = 0;
                }

                if (Input.GetKey(KeyCode.Alpha3))
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        for (int k = -1; k <= 1; k++)
                        {
                            for (int l = -1; l <= 1; l++)
                            {
                                SetBlockInWorld(d3 + j, d4 + k, d5 + l, RegisteredBlocks.Stone);
                            }
                        }
                    }

                    kappa = 0;
                }

                // if (Input.GetKey(KeyCode.Alpha6))
                // {
                //
                //     SetBlockInWorld(d3,d4,d5,RegisteredBlocks.Stone);
                //   
                //     kappa = 0;
                // }
                //
                // if (Input.GetKey(KeyCode.Alpha4))
                // {
                //
                //     SetBlockInWorld(d3,d4,d5,RegisteredBlocks.Log);
                //     // BuildBlock(raycastHit,Blocks.Stone);
                //     kappa = 0;
                // }
                //
                // if (Input.GetKey(KeyCode.Alpha5))
                // {
                //
                //     SetBlockInWorld(d3,d4,d5,RegisteredBlocks.StoneLava);
                //     // BuildBlock(raycastHit,Blocks.Stone);
                //     kappa = 0;
                // }
            }
        }


        public bool TryGetBlock(int worldX, int worldY, int worldZ, out BaseBlock? voxelData)
        {
            VoxelPosition blockCoordinates = GetFromWorldToBlockCoordinates(worldX, worldY, worldZ);
            VoxelPosition chunkCoordinates = GetFromWorldToChunkCoordinates(worldX, worldY, worldZ);


            if (chunkCoordinates.X < 0 || chunkCoordinates.X >= worldDimensions.x ||
                chunkCoordinates.Y < 0 || chunkCoordinates.Y >= worldDimensions.y ||
                chunkCoordinates.Z < 0 || chunkCoordinates.Z >= worldDimensions.z)
            {
                voxelData = null;
                return false;
            }

            if (blockCoordinates.X < 0 || blockCoordinates.X >= chunkDimensions.x ||
                blockCoordinates.Y < 0 || blockCoordinates.Y >= chunkDimensions.y ||
                blockCoordinates.Z < 0 || blockCoordinates.Z >= chunkDimensions.z)
            {
                voxelData = null;
                return false;
            }


            Chunk c = _chunks[chunkCoordinates.X, chunkCoordinates.Y, chunkCoordinates.Z];
            voxelData = c.GetVoxelData(blockCoordinates.X, blockCoordinates.Y, blockCoordinates.Z);
            return true;
        }

        private VoxelPosition GetFromWorldToBlockCoordinates(int worldX, int worldY, int worldZ)
        {
            int bx = worldX % chunkDimensions.x;
            int by = worldY % chunkDimensions.y;
            int bz = worldZ % chunkDimensions.z;

            return new VoxelPosition(bx, by, bz);
        }

        private VoxelPosition GetFromWorldToChunkCoordinates(int worldX, int worldY, int worldZ)
        {
            int cx = worldX / chunkDimensions.x;
            int cy = worldY / chunkDimensions.y;
            int cz = worldZ / chunkDimensions.z;

            return new VoxelPosition(cx, cy, cz);
        }

        private bool TryGetChunk(int x, int y, int z, out Chunk c)
        {
            if (x < 0 || x >= worldDimensions.x ||
                y < 0 || y >= worldDimensions.y ||
                z < 0 || z >= worldDimensions.z)
            {
                c = null;
                return false;
            }


            c = _chunks[x, y, z];
            return true;
        }


        public void SetBlockInWorld(VoxelPosition position, BaseBlock block)
        {
            SetBlockInWorld(position.X, position.Y, position.Z, block);
        }

        public void SetBlockInWorld(int x, int y, int z, BaseBlock baseBlock)
        {
            int cx = x >> 3;
            int cy = y >> 3;
            int cz = z >> 3;

            if (cx < 0 || cx > worldDimensions.x - 1 || cy < 0 || cy > worldDimensions.y - 1 || cz < 0 ||
                cz > worldDimensions.z - 1)
            {
                return;
            }

            int bx = x % chunkDimensions.x;
            int by = y % chunkDimensions.y;
            int bz = z % chunkDimensions.z;

            bool couldPlace = _chunks[cx, cy, cz].SetVoxel(bx, by, bz, baseBlock);

            if (couldPlace)
            {
                UpdateChunks(@by, cx, cy, cz, bx, bz);
            }
        }


        public void RefreshAllChunks()
        {
            for (var x = 0; x < worldDimensions.x; x++)
            {
                for (int y = 0; y < worldDimensions.y; y++)
                {
                    for (int z = 0; z < worldDimensions.z; z++)
                    {
                        _chunks[x, y, z].SetForRefresh();
                    }
                }
            }
        }

        private void UpdateChunks(int by, int cx, int cy, int cz, int bx, int bz)
        {
            if (@by == chunkDimensions.y - 1)
            {
                if (TryGetChunk(cx, cy + 1, cz, out Chunk chunk))
                {
                    chunk.SetForRefresh();
                }
            }
            else if (@by == 0)
            {
                if (TryGetChunk(cx, cy - 1, cz, out Chunk chunk))
                {
                    chunk.SetForRefresh();
                }
            }


            if (bx == chunkDimensions.x - 1)
            {
                if (TryGetChunk(cx + 1, cy, cz, out Chunk chunk))
                {
                    chunk.SetForRefresh();
                }

                ;
            }
            else if (bx == 0)
            {
                if (TryGetChunk(cx - 1, cy, cz, out Chunk chunk))
                {
                    chunk.SetForRefresh();
                }

                ;
            }

            if (bz == 0)

            {
                if (TryGetChunk(cx, cy, cz - 1, out Chunk chunk))
                {
                    chunk.SetForRefresh();
                }

                ;
            }
            else if (bz == chunkDimensions.z - 1)

            {
                if (TryGetChunk(cx, cy, cz + 1, out Chunk chunk))
                {
                    chunk.SetForRefresh();
                }

                ;
            }
        }

        private Vector3Int blockPos;

        Stopwatch stoppy = new();

        public IEnumerator Createworld()
        {
            stoppy.Start();
            _chunks = new Chunk[worldDimensions.x, worldDimensions.y, worldDimensions.z];

            for (int x = 0; x < worldDimensions.x; x++)
            {
                for (int y = 0; y < worldDimensions.y; y++)
                {
                    for (int z = 0; z < worldDimensions.z; z++)
                    {
                        GameObject chunk = Instantiate(chunkPrefab);
                        Chunk c = chunk.GetComponent<Chunk>();
                        Vector3Int position = new Vector3Int(x * chunkDimensions.x, y * chunkDimensions.y,
                            z * chunkDimensions.z);

                        c.worldChunking = this;
                        _chunks[x, y, z] = c;
                        c._textureAtlas = _textureAtlas;
                        c.atlas.mainTexture = _textureAtlas.Atlas;
                        c.AllocateChunk(chunkDimensions, position);
                        c.InitializeChunk();
                    }
                }
            }

            for (int i = 0; i < worldDimensions.x * chunkDimensions.x; i++)
            {
                for (int j = 0; j < worldDimensions.z * chunkDimensions.z; j++)
                {
                    SetBlockInWorld(i, 0, j, RegisteredBlocks.Stone);
                }
            }

            RefreshAllChunks();
            stoppy.Stop();
            Debug.Log("Takes" + stoppy.ElapsedMilliseconds);
            yield return null;
        }
    }
}