using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using System.Runtime.InteropServices;
using System;

public class ArrayPointerTest : MonoBehaviour
{
    unsafe void Start()
    {
        var array = new Vector3 [0x100];

        var handle1 = GCHandle.Alloc(array, GCHandleType.Pinned);
        var pointer1 = handle1.AddrOfPinnedObject();

        ulong handle2 = 0;
        var pointer2 = (IntPtr)UnsafeUtility.PinGCObjectAndGetAddress(array, out handle2);

        var pointer3 = (IntPtr)UnsafeUtility.AddressOf(ref array[0]);

        Debug.Log("Comparison between pointers of an array acquired with different functions.");
        Debug.Log("1: " + pointer1.ToString("X") + " (System.Runtime.InteropServices.GCHandle)");
        Debug.Log("2: " + pointer2.ToString("X") + " (UnsafeUtility.PinGCObjectAndGetAddress)");
        Debug.Log("3: " + pointer3.ToString("X") + " (UnsafeUtility.AddressOf)");

        handle1.Free();
        UnsafeUtility.ReleaseGCObject(handle2);
    }
}
