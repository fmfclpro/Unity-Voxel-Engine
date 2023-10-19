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

namespace FMFCLPRO.FMatrices
{
    public struct FMatrix3x3
    {
        private readonly float[,] _matrix;

        public FMatrix3x3(float m00, float m10, float m20, float m01, float m11, float m21, float m02, float m12,
            float m22)
        {
            _matrix = new float[3, 3];
            _matrix[0, 0] = m00;
            _matrix[1, 0] = m10;
            _matrix[2, 0] = m20;
            _matrix[0, 1] = m01;
            _matrix[1, 1] = m11;
            _matrix[2, 1] = m21;
            _matrix[0, 2] = m02;
            _matrix[1, 2] = m12;
            _matrix[2, 2] = m22;
        }

        public Vector3 MultiplyWithVector3(Vector3 point)
        {
            float f0 = _matrix[0, 0] * point.x;
            float f1 = _matrix[1, 0] * point.y;
            float f2 = _matrix[2, 0] * point.z;
            float f3 = _matrix[0, 1] * point.x;
            float f4 = _matrix[1, 1] * point.y;
            float f5 = _matrix[2, 1] * point.z;
            float f6 = _matrix[0, 2] * point.x;
            float f7 = _matrix[1, 2] * point.y;
            float f8 = _matrix[2, 2] * point.z;
            return new Vector3(f0 + f1 + f2, f3 + f4 + f5, f6 + f7 + f8);
        }
    }
}