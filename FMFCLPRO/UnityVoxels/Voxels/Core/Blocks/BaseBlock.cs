using System.Collections.Generic;
using FMFCLPRO.Registry;
using FMFCLPRO.TagProperties;
using FMFCLPRO.UnityVoxels.Voxels.Atlas;
using FMFCLPRO.UnityVoxels.Voxels.Core.BlockDatas;
using FMFCLPRO.UnityVoxels.Voxels.Core.BlockProperties;
using FMFCLPRO.UnityVoxels.Voxels.Core.World;
using FMFCLPRO.UnityVoxels.Voxels.Shapes;
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


namespace FMFCLPRO.UnityVoxels.Voxels.Core.Blocks
{
    public class CustomShape : IVoxelShape
    {
        public void Render(WorldChunking worldChunking, Vector3 offset, BaseBlock inBlock, Chunk chunk,
            TextureAtlas textureAtlas)
        {
            Mesh = worldChunking.GetCustomMesh();
        }

        public Mesh Mesh { get; set; }
    }

    public class AirBlock : BaseBlock
    {
        public AirBlock(BlockProperty blockProperty) : base(blockProperty)
        {
        }

        public override IVoxelShape RenderShape()
        {
            return new VoxelCube();
        }

        public override BlockShapeType BlockShapeType { get; }
    }

    public class Block : BaseBlock
    {
        private IVoxelShape _shape = new VoxelCube();

        public override IVoxelShape RenderShape()
        {
            return _shape;
        }

        public override void OnBlockDestroy(VoxelPosition blockPos, WorldChunking worldChunking)
        {
            worldChunking.SetBlockInWorld(blockPos, RegisteredBlocks.Air);
            base.OnBlockDestroy(blockPos, worldChunking);
        }

        public override BlockShapeType BlockShapeType => BlockShapeType.Block;

        public Block(BlockProperty blockProperty) : base(blockProperty)
        {
            AddTagProperty("health", new ByteTagVariable("health", 5));
        }
    }

    public class CustomBlock : BaseBlock
    {
        public CustomBlock(BlockProperty blockProperty) : base(blockProperty)
        {
        }

        public override IVoxelShape RenderShape()
        {
            return new CustomShape();
        }

        public override BlockShapeType BlockShapeType { get; }
    }

    public abstract class BaseBlock
    {
        private BlockProperty _blockProperty;
        public Dictionary<string, ITagVariable> blockVariables = new Dictionary<string, ITagVariable>();

        public BaseBlock(BlockProperty blockProperty)
        {
            _blockProperty = blockProperty;
        }

        public void AddTagProperty(string tagID, ITagVariable tagVariable)
        {
            blockVariables.Add(tagID, tagVariable);
        }

        public virtual void OnBlockUse()
        {
        }

        public virtual void OnBlockDestroy(VoxelPosition blockPos, WorldChunking worldChunking)
        {
        }

        public BlockInstance GetInstance()
        {
            return new BlockInstance(this);
        }

        public BlockProperty GetBlockProperty()
        {
            return _blockProperty;
        }

        public bool IsAir()
        {
            BlockProperty blockProperties = _blockProperty;
            return blockProperties.IsAir;
        }

        public abstract IVoxelShape RenderShape();
        public ushort ID { get; set; }
        public abstract BlockShapeType BlockShapeType { get; }
    }
}