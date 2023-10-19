using System.Collections.Generic;
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

namespace FMFCLPRO.UnityVoxels.Voxels.Atlas
{
    [System.Serializable]
    public class TextureAtlas
    {
        public Dictionary<string, Vector2[,]> tagToUv = new Dictionary<string, Vector2[,]>();

        public Texture2D Atlas;

        public Vector2[,] GetUVCoordinateFromTag(string tag)
        {
            return tagToUv[tag];
        }

        public Vector2[,] get_texture(int x, int y)
        {
            float n = 1 / 16f;

            Vector2 firstPoint = new Vector2(n + (n * x), n + (n * y));
            Vector2 secondPoint = new Vector2(n * x, n + (n * y));
            Vector2 thirdPoint = new Vector2(n * x, n * y);
            Vector2 fourth = new Vector2(n + (n * x), n * y);

            Vector2[,] uvs = new Vector2[2, 2];
            uvs[0, 0] = firstPoint;
            uvs[1, 0] = secondPoint;
            uvs[0, 1] = thirdPoint;
            uvs[1, 1] = fourth;
            return uvs;
        }
    }
}