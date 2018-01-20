using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;

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
        na.CopyTo(_buffer);
        na.Dispose();
    }
}
