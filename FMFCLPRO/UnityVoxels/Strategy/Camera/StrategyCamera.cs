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

namespace FMFCLPRO.UnityVoxels.Strategy.Camera
{
[RequireComponent(typeof(UnityEngine.Camera))]
public class StrategyCamera : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(transform.forward.x, 0, transform.forward.z);
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(-transform.forward.x, 0, -transform.forward.z);
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-transform.right.x, 0, -transform.right.z);
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(transform.right.x, 0, transform.right.z);
            
        }
        if (Input.GetKey(KeyCode.F))
        {
            transform.position += Vector3.up;
            
        }
        if (Input.GetKey(KeyCode.G))
        {
            transform.position += -Vector3.up;
            
        }
        if (Input.GetKey(KeyCode.E))
        {
            var rot = transform.rotation.eulerAngles;
            rot.y += 100 * Time.deltaTime;
            transform.eulerAngles = rot;
            

        }
        if (Input.GetKey(KeyCode.Q))
        {
            var rot = transform.rotation.eulerAngles;
            rot.y -= 100 * Time.deltaTime;
            transform.eulerAngles = rot;
            

        }
    }
}

}
