using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.IL2CPP.CompilerServices;
 
/// <summary>
/// Extension methods to <see cref="NativeArray{T}"/>
/// </summary>
/// <author>
/// Jackson Dunstan, https://jacksondunstan.com/articles/4713
/// </author>
public static class NativeArrayExtensions
{
	/// <summary>
	/// A faster version of <see cref="NativeArray{T}.CopyFrom(T[])"/>
	/// </summary>
	/// 
	/// <param name="nativeArray">
	/// <see cref="NativeArray{T}"/> to copy from
	/// </param>
	///
	/// <param name="array">
	/// Managed array to copy to
	/// </param>
	///
	/// <typeparam name="T">
	/// Type of elements in the <see cref="NativeArray{T}"/> and managed array
	/// </typeparam>
	///
	/// <exception cref="NullReferenceException">
	/// Thrown if the managed array is null
	/// </exception>
	///
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown if the managed array is shorter than the
	/// <see cref="NativeArray{T}"/>
	/// </exception>
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public unsafe static void CopyFromFast<T>(
		this NativeArray<T> nativeArray,
		T[] array)
		where T : struct
	{
		if (array == null)
		{
			throw new NullReferenceException(nameof(array) + " is null");
		}
 
		int nativeArrayLength = nativeArray.Length;
		if (array.Length < nativeArrayLength)
		{
			throw new IndexOutOfRangeException(
				nameof(array) + " is shorter than " + nameof(nativeArray));
		}
		void* buffer = nativeArray.GetUnsafePtr();
		for (int i = 0; i < nativeArrayLength; ++i)
		{
			UnsafeUtility.WriteArrayElement(buffer, i, array[i]);
		}
	}
 
	/// <summary>
	/// A faster version of <see cref="NativeArray{T}.CopyTo(T[])"/>
	/// </summary>
	/// 
	/// <param name="nativeArray">
	/// <see cref="NativeArray{T}"/> to copy to
	/// </param>
	///
	/// <param name="array">
	/// Managed array to copy from
	/// </param>
	///
	/// <typeparam name="T">
	/// Type of elements in the <see cref="NativeArray{T}"/> and managed array
	/// </typeparam>
	///
	/// <exception cref="NullReferenceException">
	/// Thrown if the managed array is null
	/// </exception>
	///
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown if the managed array is shorter than the
	/// <see cref="NativeArray{T}"/>
	/// </exception>
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public unsafe static void CopyToFast<T>(
		this NativeArray<T> nativeArray,
		T[] array)
		where T : struct
	{
		if (array == null)
		{
			throw new NullReferenceException(nameof(array) + " is null");
		}
 
		int nativeArrayLength = nativeArray.Length;
		if (array.Length < nativeArrayLength)
		{
			throw new IndexOutOfRangeException(
				nameof(array) + " is shorter than " + nameof(nativeArray));
		}
		void* buffer = nativeArray.GetUnsafePtr();
		for (int i = 0; i < nativeArrayLength; ++i)
		{
			array[i] = UnsafeUtility.ReadArrayElement<T>(buffer, i);
		}
	}
}
