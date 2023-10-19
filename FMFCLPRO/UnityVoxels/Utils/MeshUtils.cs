using System;
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

namespace FMFCLPRO.UnityVoxels.Utils
{
    public static class MeshUtils
    {
        public static Mesh MergeMeshes(Mesh[] meshes)
        {
            Mesh mesh = new Mesh();

            Dictionary<Tuple<Vector3, Vector3, Vector2>, int> pointsOrder =
                new Dictionary<Tuple<Vector3, Vector3, Vector2>, int>();
            HashSet<Tuple<Vector3, Vector3, Vector2>> pointsHash = new HashSet<Tuple<Vector3, Vector3, Vector2>>();
            List<int> tris = new List<int>();

            int pIndex = 0;
            for (int i = 0; i < meshes.Length; i++) //loop through each mesh
            {
                if (meshes[i] == null) continue;
                for (int j = 0; j < meshes[i].vertices.Length; j++) //loop through each vertex of the current mesh
                {
                    Vector3 v = meshes[i].vertices[j];
                    Vector3 n = meshes[i].normals[j];
                    Vector2 u = meshes[i].uv[j];
                    Tuple<Vector3, Vector3, Vector2> p = new Tuple<Vector3, Vector3, Vector2>(v, n, u);
                    if (!pointsHash.Contains(p))
                    {
                        pointsOrder.Add(p, pIndex);
                        pointsHash.Add(p);

                        pIndex++;
                    }
                }

                for (int t = 0; t < meshes[i].triangles.Length; t++)
                {
                    int triPoint = meshes[i].triangles[t];
                    Vector3 v = meshes[i].vertices[triPoint];
                    Vector3 n = meshes[i].normals[triPoint];
                    Vector2 u = meshes[i].uv[triPoint];
                    Tuple<Vector3, Vector3, Vector2> p = new Tuple<Vector3, Vector3, Vector2>(v, n, u);

                    int index;
                    pointsOrder.TryGetValue(p, out index);
                    tris.Add(index);
                }

                meshes[i] = null;
            }

            ExtractArrays(pointsOrder, mesh);
            mesh.triangles = tris.ToArray();
            mesh.RecalculateBounds();
            return mesh;
        }

        public static void ExtractArrays(Dictionary<Tuple<Vector3, Vector3, Vector2>, int> list, Mesh mesh)
        {
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> norms = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();

            foreach (Tuple<Vector3, Vector3, Vector2> v in list.Keys)
            {
                verts.Add(v.Item1);
                norms.Add(v.Item2);
                uvs.Add(v.Item3);
            }

            mesh.vertices = verts.ToArray();
            mesh.normals = norms.ToArray();
            mesh.uv = uvs.ToArray();
        }
    }
}