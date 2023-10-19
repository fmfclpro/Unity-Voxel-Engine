using System.Collections.Generic;
using FMFCLPRO.UnityVoxels.Voxels.Core.BlockProperties;
using FMFCLPRO.UnityVoxels.Voxels.Core.Blocks;

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


namespace FMFCLPRO.Registry
{
    public static class RegisteredBlocks
    {
        public static readonly Dictionary<ushort, BaseBlock> BlockDatas = new Dictionary<ushort, BaseBlock>();
        public static readonly Dictionary<ushort, string> IDToTag = new Dictionary<ushort, string>();

        public static readonly BaseBlock Air =
            RegisterNewVoxel("air", new Block(new BlockProperty(BlockMaterial.AIR_MATERIAL).Air()));

        public static readonly BaseBlock Stone =
            RegisterNewVoxel("fake_stone", new Block(new BlockProperty(BlockMaterial.STONE)));

        public static readonly BaseBlock Dirt =
            RegisterNewVoxel("dirt", new Block(new BlockProperty(BlockMaterial.STONE)));

        public static readonly BaseBlock Red =
            RegisterNewVoxel("red", new Block(new BlockProperty(BlockMaterial.STONE)));

        public static readonly BaseBlock Custom =
            RegisterNewVoxel("custom", new CustomBlock(new BlockProperty(BlockMaterial.STONE)));

        private static ushort blockID;

        static BaseBlock RegisterNewVoxel(string tag, BaseBlock baseBlock)
        {
            baseBlock.ID = blockID;
            BlockDatas[blockID] = baseBlock;
            IDToTag[blockID] = tag;

            blockID++;
            return baseBlock;
        }
    }
}