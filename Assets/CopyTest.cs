using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using System.Runtime.InteropServices;

public class CopyTest : MonoBehaviour
{
    byte[] _buffer;

    void Start()
    {
        _buffer = new byte[1920 * 1080 * 4];
    }

    void Update()
    {
        var na = new NativeArray<byte>(_buffer.Length, Allocator.Temp);

        Marshal.Copy(na.GetUnsafePtr(), _buffer, 0, _buffer.Length);

        //na.CopyTo(_buffer);

        na.Dispose();
    }
}
