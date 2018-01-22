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

        // Use NativeArray.CopyTo (naive implementation; very slow)
        //na.CopyTo(_buffer);

        // Use Marshal to copy from unmanaged to managed (fast)
        Marshal.Copy(na.GetUnsafePtr(), _buffer, 0, _buffer.Length);

        // Use GCHandle to retrieve pointer to managed array (fast)
        //var gch = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
        //UnsafeUtility.MemCpy(gch.AddrOfPinnedObject(), na.GetUnsafePtr(), (ulong)_buffer.Length);
        //gch.Free();

        na.Dispose();
    }
}
