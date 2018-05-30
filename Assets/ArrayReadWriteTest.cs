using UnityEngine;
using UnityEngine.Assertions;
using Unity.Collections.LowLevel.Unsafe;

public class ArrayReadWriteTest : MonoBehaviour
{
    unsafe void Start()
    {
        var array = new Vector3 [100];
        var pointer = UnsafeUtility.AddressOf(ref array[0]);

        var src1 = new Vector3(1, 2, 3) * Mathf.PI;
        var src2 = new Vector3(4, 5, 6) * Mathf.PI;

        UnsafeUtility.WriteArrayElement(pointer, 20, src1);
        array[30] = src2;

        var ret1 = array[20];
        var ret2 = UnsafeUtility.ReadArrayElement<Vector3>(pointer, 30);

        Assert.AreEqual(src1, ret1);
        Assert.AreEqual(src2, ret2);
    }
}
