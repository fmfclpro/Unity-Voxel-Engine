using System.Collections.Generic;
using FMFCLPRO.Registry;
using FMFCLPRO.UnityVoxels.Resources;
using FMFCLPRO.UnityVoxels.Utils;
using FMFCLPRO.UnityVoxels.Voxels.Atlas;
using FMFCLPRO.UnityVoxels.Voxels.Core;
using FMFCLPRO.UnityVoxels.Voxels.Core.BlockProperties;
using FMFCLPRO.UnityVoxels.Voxels.Core.Blocks;
using FMFCLPRO.UnityVoxels.Voxels.Core.World;
using FMFCLPRO.Voxels.Core;
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


namespace FMFCLPRO.UnityVoxels.Voxels.Shapes
{
    public class VoxelCube : IVoxelShape
    {
        private Chunk parentChunk;
        private VoxelPosition chunloca;

        public void Render(WorldChunking worldChunking, Vector3 offset, BaseBlock inBlock, Chunk chunk,
            TextureAtlas textureAtlas)
        {
            parentChunk = chunk;
            VoxelPosition blockLocalPos = offset - chunk.location;
            chunloca = chunk.location;

            if (!inBlock.IsAir())
            {
                string tag = RegisteredBlocks.IDToTag[inBlock.ID];

                VoxelTextureData a = Initialiser.TextureDatas[tag];

                Vector2[,] pointsUP = textureAtlas.GetUVCoordinateFromTag(a.up);
                Vector2[,] pointsDown = textureAtlas.GetUVCoordinateFromTag(a.down);
                Vector2[,] pointsLeft = textureAtlas.GetUVCoordinateFromTag(a.left);
                Vector2[,] pointsRight = textureAtlas.GetUVCoordinateFromTag(a.right);
                Vector2[,] pointsBackawards = textureAtlas.GetUVCoordinateFromTag(a.backward);
                Vector2[,] pointsForwaRD = textureAtlas.GetUVCoordinateFromTag(a.forward);

                List<IShape> quads = new List<IShape>();

                if (!HasSolidNeighbour(worldChunking, inBlock, blockLocalPos, VoxelSide.Down))
                    quads.Add(new Quad(VoxelSide.Down, offset, pointsDown));
                if (!HasSolidNeighbour(worldChunking, inBlock, blockLocalPos, VoxelSide.Up))
                    quads.Add(new Quad(VoxelSide.Up, offset, pointsUP));
                if (!HasSolidNeighbour(worldChunking, inBlock, blockLocalPos, VoxelSide.Left))
                    quads.Add(new Quad(VoxelSide.Left, offset, pointsLeft));
                if (!HasSolidNeighbour(worldChunking, inBlock, blockLocalPos, VoxelSide.Right))
                    quads.Add(new Quad(VoxelSide.Right, offset, pointsRight));
                if (!HasSolidNeighbour(worldChunking, inBlock, blockLocalPos, VoxelSide.Forward))
                    quads.Add(new Quad(VoxelSide.Forward, offset, pointsForwaRD));
                if (!HasSolidNeighbour(worldChunking, inBlock, blockLocalPos, VoxelSide.Back))
                    quads.Add(new Quad(VoxelSide.Back, offset, pointsBackawards));

                if (quads.Count == 0) return;
                Mesh[] sideMeshes = new Mesh[quads.Count];
                int m = 0;
                foreach (IShape q in quads)
                {
                    sideMeshes[m] = q.GetMesh();
                    m++;
                }

                Mesh = MeshUtils.MergeMeshes(sideMeshes);
            }
        }

        public Mesh Mesh { get; set; }
        public BlockShapeType BlockShapeType => BlockShapeType.Block;


        bool HasSolidNeighbour(WorldChunking worldChunking, BaseBlock inBlock, VoxelPosition position, VoxelSide side)
        {
            VoxelPosition p = position.GetDirection(side);
            return HasSolidNeighbour(worldChunking, inBlock, p.X, p.Y, p.Z);
        }

        bool HasSolidNeighbour(WorldChunking worldChunking, BaseBlock inBlock, int x, int y, int z)
        {
            if (x < 0 || x >= parentChunk.width ||
                y < 0 || y >= parentChunk.height ||
                z < 0 || z >= parentChunk.depth)
            {
                x += chunloca.X;
                y += chunloca.Y;
                z += chunloca.Z;
                bool exists = worldChunking.TryGetBlock(x, y, z, out BaseBlock? v);
                if (exists)
                {
                    if (v.IsAir() || (v.GetBlockProperty().BlockMaterial.BlockMaterState != BlockMaterState.Solid))
                        return false;

                    return true;
                }

                return false;
            }


            BaseBlock block = parentChunk.GetVoxelData(x, y, z);

            if (block.IsAir() || (block.GetBlockProperty().BlockMaterial.BlockMaterState != BlockMaterState.Solid))
                return false;
            return true;
        }
    }
}