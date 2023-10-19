using System;
using FMFCLPRO.FMatrices;
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
    public class VoxelQuad : IShape
    {
        private Mesh _mesh;

        public VoxelQuad(VoxelSide side, Vector3 offset, Vector2[,] uvPoints)
        {
            CreateQuad(side, Vector3.zero, offset, uvPoints);
        }

        public VoxelQuad(VoxelSide side, Vector3 rotation, Vector3 offset, Vector2[,] uvPoints)
        {
            CreateQuad(side, rotation, offset, uvPoints);
        }

        private Vector3 RotatePoint(Vector3 toRotate, Vector3 rotation)
        {
            rotation.x = Math.Geometry.ToRadian(rotation.x);
            rotation.y = Math.Geometry.ToRadian(rotation.y);
            rotation.z = Math.Geometry.ToRadian(rotation.z);

            FMatrix3x3 rotationX = new FMatrix3x3(1, 0, 0, 0, MathF.Cos(rotation.x), -MathF.Sin(rotation.x), 0,
                MathF.Sin(rotation.x), MathF.Cos(rotation.x));
            FMatrix3x3 rotationZ = new FMatrix3x3(MathF.Cos(rotation.z), -MathF.Sin(rotation.z), 0,
                MathF.Sin(rotation.z), MathF.Cos(rotation.z), 0, 0, 0, 1);
            FMatrix3x3 rotationY = new FMatrix3x3(MathF.Cos(rotation.y), 0, MathF.Sin(rotation.y), 0, 1, 0,
                -MathF.Sin(rotation.y), 0, MathF.Cos(rotation.y));

            Vector3 tr = rotationX.MultiplyWithVector3(toRotate);
            Vector3 tr2 = rotationY.MultiplyWithVector3(tr);
            Vector3 tr3 = rotationZ.MultiplyWithVector3(tr2);

            return tr3;
        }

        float toNormal(float angle)
        {
            return (MathF.PI / 180) * angle;
        }

        private void CreateQuad(VoxelSide side, Vector3 rotation, Vector3 offset, Vector2[,] uvPoints)
        {
            bool hasRotation = rotation != Vector3.zero;

            _mesh = new Mesh();
            Vector3[] vertices = new Vector3[4];
            Vector3[] normals = new Vector3[4];
            Vector2[] uvs = new Vector2[4];
            int[] triangles = new int[6];
            triangles = new[] { 3, 1, 0, 3, 2, 1 };

            Vector2 uv11 = uvPoints[0, 0] + new Vector2(-0.001f, -0.001f);
            Vector2 uv01 = uvPoints[1, 0] + new Vector2(+0.001f, -0.001f);
            Vector2 uv00 = uvPoints[0, 1] + new Vector2(+0.001f, +0.001f);
            Vector2 uv10 = uvPoints[1, 1] + new Vector2(-0.001f, +0.001f);

            Vector3 p0 = new Vector3(-0.5f / 2f, -0.5f / 2f, 0.5f);
            Vector3 p1 = new Vector3(0.5f / 2f, -0.5f / 2f, 0.5f);
            Vector3 p2 = new Vector3(0.5f / 2f, -0.5f / 2f, -0.5f);
            Vector3 p3 = new Vector3(-0.5f / 2f, -0.5f / 2f, -0.5f);
            Vector3 p4 = new Vector3(-0.5f / 2f, 0.5f / 2f, 0.5f);
            Vector3 p5 = new Vector3(0.5f / 2f, 0.5f / 2f, 0.5f);
            Vector3 p6 = new Vector3(0.5f / 2f, 0.5f / 2f, -0.5f);
            Vector3 p7 = new Vector3(-0.5f / 2f, 0.5f / 2f, -0.5f);

            switch (side)
            {
                case VoxelSide.Down:

                    if (hasRotation)
                    {
                        p0 = RotatePoint(p0, rotation);
                        p1 = RotatePoint(p1, rotation);
                        p2 = RotatePoint(p2, rotation);
                        p3 = RotatePoint(p3, rotation);
                    }

                    vertices = new[] { p0 + offset, p1 + offset, p2 + offset, p3 + offset };
                    normals = new[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                    uvs = new[] { uv11, uv01, uv00, uv10 };

                    break;
                case VoxelSide.Up:

                    if (hasRotation)
                    {
                        p7 = RotatePoint(p7, rotation);
                        p6 = RotatePoint(p6, rotation);
                        p5 = RotatePoint(p5, rotation);
                        p4 = RotatePoint(p4, rotation);
                    }

                    vertices = new[] { p7 + offset, p6 + offset, p5 + offset, p4 + offset };
                    normals = new[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                    uvs = new[] { uv11, uv01, uv00, uv10 };

                    break;
                case VoxelSide.Left:

                    if (hasRotation)
                    {
                        p7 = RotatePoint(p7, rotation);
                        p4 = RotatePoint(p4, rotation);
                        p0 = RotatePoint(p0, rotation);
                        p3 = RotatePoint(p3, rotation);
                    }

                    vertices = new[] { p7 + offset, p4 + offset, p0 + offset, p3 + offset };
                    normals = new[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                    uvs = new[] { uv11, uv01, uv00, uv10 };

                    break;
                case VoxelSide.Right:

                    if (hasRotation)
                    {
                        p5 = RotatePoint(p5, rotation);
                        p6 = RotatePoint(p6, rotation);
                        p2 = RotatePoint(p2, rotation);
                        p1 = RotatePoint(p1, rotation);
                    }

                    vertices = new[] { p5 + offset, p6 + offset, p2 + offset, p1 + offset };
                    normals = new[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                    uvs = new[] { uv11, uv01, uv00, uv10 };

                    break;
                case VoxelSide.Forward:

                    if (hasRotation)
                    {
                        p4 = RotatePoint(p4, rotation);
                        p5 = RotatePoint(p5, rotation);
                        p1 = RotatePoint(p1, rotation);
                        p0 = RotatePoint(p0, rotation);
                    }

                    vertices = new[] { p4 + offset, p5 + offset, p1 + offset, p0 + offset };
                    normals = new[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                    uvs = new[] { uv11, uv01, uv00, uv10 };

                    break;
                case VoxelSide.Back:
                    if (hasRotation)
                    {
                        p6 = RotatePoint(p6, rotation);
                        p7 = RotatePoint(p7, rotation);
                        p3 = RotatePoint(p3, rotation);
                        p2 = RotatePoint(p2, rotation);
                    }

                    vertices = new[] { p6 + offset, p7 + offset, p3 + offset, p2 + offset };
                    normals = new[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                    uvs = new[] { uv11, uv01, uv00, uv10 };

                    break;
            }

            _mesh.vertices = vertices;
            _mesh.normals = normals;
            _mesh.uv = uvs;
            _mesh.triangles = triangles;
            _mesh.RecalculateBounds();
        }

        public Mesh GetMesh()
        {
            return _mesh;
        }
    }
}