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

namespace FMFCLPRO.UnityVoxels.Voxels.Shapes
{
    public class VerticalSlabQuad : IShape
    {
        public Mesh mesh;

        public VerticalSlabQuad(VoxelSide side, Vector3 offset, Vector2[,] uvPoints)
        {
            CreateQuad(side, offset, uvPoints);
        }
	
        private void CreateQuad(VoxelSide side, Vector3 offset, Vector2[,] uvPoints)
        {
            mesh = new Mesh();
            Vector3[] vertices = new Vector3[4];
            Vector3[] normals = new Vector3[4];
            Vector2[] uvs = new Vector2[4];
            int[] triangles = new int[6];
            triangles = new[] {3, 1, 0, 3, 2, 1};

            Vector2 uv11 = uvPoints[0, 0] + new Vector2(-0.001f,-0.001f);
            Vector2 uv01 = uvPoints[1, 0] + new Vector2(+0.001f, -0.001f);
            Vector2 uv00 = uvPoints[0, 1] + new Vector2(+0.001f, +0.001f);
            Vector2 uv10 = uvPoints[1, 1] + new Vector2(-0.001f, +0.001f);

             
            Vector3 p0 = new Vector3(-0.5f, -0.5f , 0.5f) + offset;
            Vector3 p1 = new Vector3(0.5f, -0.5f , 0.5f) + offset;
            Vector3 p2 = new Vector3(0.5f, -0.5f, 0f) + offset;
            Vector3 p3 = new Vector3(-0.5f, -0.5f, -0f) + offset;
            Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f) + offset;
            Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f) + offset;
            Vector3 p6 = new Vector3(0.5f, 0.5f, -0) + offset;
            Vector3 p7 = new Vector3(-0.5f, 0.5f, -0) + offset;

            switch (side)
            {
                case VoxelSide.Down:
                    vertices = new[] {p0, p1, p2, p3};
                    normals = new[]
                    {
                        Vector3.down, Vector3.down,
                        Vector3.down, Vector3.down
                    };
                    uvs = new[] {uv11, uv01, uv00, uv10};

                    break;
                case VoxelSide.Up:
                    vertices = new[] {p7, p6, p5, p4};
                    normals = new[]
                    {
                        Vector3.up, Vector3.up,
                        Vector3.up, Vector3.up
                    };
                    uvs = new[] {uv11, uv01, uv00, uv10};

                    break;
                case  VoxelSide.Left:
                    vertices = new[] {p7, p4, p0, p3};
                    normals = new[]
                    {
                        Vector3.left, Vector3.left,
                        Vector3.left, Vector3.left
                    };
                    uvs = new[] {uv11, uv01, uv00, uv10};

                    break;
                case VoxelSide.Right:
                    vertices = new[] {p5, p6, p2, p1};
                    normals = new[]
                    {
                        Vector3.right, Vector3.right,
                        Vector3.right, Vector3.right
                    };
                    uvs = new[] {uv11, uv01, uv00, uv10};

                    break;
                case VoxelSide.Forward:
                    vertices = new[] {p4, p5, p1, p0};
                    normals = new[]
                    {
                        Vector3.forward, Vector3.forward,
                        Vector3.forward, Vector3.forward
                    };
                    uvs = new[] {uv11, uv01, uv00, uv10};

                    break;
                case VoxelSide.Back:
                    vertices = new[] {p6, p7, p3, p2};
                    normals = new[]
                    {
                        Vector3.back, Vector3.back,
                        Vector3.back, Vector3.back
                    };
                    uvs = new[] {uv11, uv01, uv00, uv10};

                    break;
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
        }

        public Mesh GetMesh()
        {
            return mesh;
        }
    }
}