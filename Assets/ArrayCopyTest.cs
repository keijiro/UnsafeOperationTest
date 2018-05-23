using UnityEngine;
using UnityEngine.Assertions;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System;

namespace ArrayCopyTest
{
    abstract class TestBase
    {
        int [] _source;

        Stopwatch _stopwatch = new Stopwatch();
        long _ticks;
        int _count;

        public TestBase(int length)
        {
            _source = new int [length];

            for (var i = 0; i < length; i++) _source[i] = i;
        }

        public abstract void DoCopy(int[] source, NativeArray<int> destination);

        public void RunTest(bool verify)
        {
            var nativeArray = new NativeArray<int>(_source.Length, Allocator.Temp);

            _stopwatch.Reset();
            _stopwatch.Start();

            DoCopy(_source, nativeArray);

            _stopwatch.Stop();
            _ticks += _stopwatch.ElapsedTicks;
            _count++;

            if (verify)
                for (var i = 0; i < _source.Length; i++)
                    Assert.AreEqual(_source[i], nativeArray[i]);

            nativeArray.Dispose();
        }

        public int CalculateAverageTicks()
        {
            var result = (int)(_ticks / _count);
            _ticks = _count = 0;
            return result;
        }
    }

    class TestMarshalCopy : TestBase
    {
        public TestMarshalCopy(int length) : base(length) {}

        unsafe public override void DoCopy(int[] source, NativeArray<int> destination)
        {
            Marshal.Copy(source, 0, (IntPtr)destination.GetUnsafePtr(), source.Length);
        }
    }

    class TestNativeCopyFrom : TestBase
    {
        public TestNativeCopyFrom(int length) : base(length) {}

        public override void DoCopy(int[] source, NativeArray<int> destination)
        {
            destination.CopyFrom(source);
        }
    }

    class TestUnsafeMemCpy : TestBase
    {
        public TestUnsafeMemCpy(int length) : base(length) {}

        unsafe public override void DoCopy(int[] source, NativeArray<int> destination)
        {
            var gch = GCHandle.Alloc(source, GCHandleType.Pinned);
            UnsafeUtility.MemCpy(
                destination.GetUnsafePtr(),
                (void*)gch.AddrOfPinnedObject(),
                sizeof(int) * source.Length
            );
            gch.Free();
        }
    }

    class TestCopyFromFast : TestBase
    {
        public TestCopyFromFast(int length) : base(length) {}

        public override void DoCopy(int[] source, NativeArray<int> destination)
        {
            destination.CopyFromFast(source);
        }
    }

    public class ArrayCopyTest : MonoBehaviour
    {
        [SerializeField] int _length = 1920 * 1080;
        [SerializeField] UnityEngine.UI.Text _text;

        TestBase[] _tests;

        void Start()
        {
            _tests = new TestBase [] {
                new TestMarshalCopy(_length),
                new TestNativeCopyFrom(_length),
                new TestUnsafeMemCpy(_length),
                new TestCopyFromFast(_length)
            };

            foreach (var test in _tests) test.RunTest(true);
        }

        void Update()
        {
            foreach (var test in _tests) test.RunTest(false);

            if ((Time.frameCount % 30) == 0)
            {
                _text.text = "";
                foreach (var test in _tests)
                    _text.text += string.Format(
                        "{0,20}:{1,8} ticks\n",
                        test.GetType().Name,
                        test.CalculateAverageTicks()
                    );
            }
        }
    }
}
