﻿#if UNITY_EDITOR

#define BOOL_CHECKS
#define NULL_CHECKS
#define FILE_PATH_CHECKS
#define BOUNDS_CHECKS
#define SUBARRAY_CHECKS
#define EQUALITY_CHECKS
#define COMPARE_CHECKS
#define BIT_SHIFT_CHECKS

#endif

using System;
using System.IO;
using System.Runtime.CompilerServices;

// CONDITIONAL ATTRIBUTE DOESN'T WORK
namespace DevTools
{
    unsafe public static class Assert
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsTrue(bool condition)
        {
#if BOOL_CHECKS
            if (!condition)
            {
                throw new Exception("Expected 'true'");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsFalse(bool condition)
        {
#if BOOL_CHECKS
            if (condition)
            {
                throw new Exception("Expected 'false'");
            }
#endif
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNull(object obj)
        {
#if NULL_CHECKS
            if (obj != null)
            {
                throw new InvalidDataException("Expected null"); ;
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNull(void* ptr)
        {
#if NULL_CHECKS
            if (ptr != null)
            {
                throw new InvalidDataException("Expected null"); ;
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotNull(object obj)
        {
#if NULL_CHECKS
            if (obj == null)
            {
                throw new NullReferenceException("Expected not-null"); ;
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotNull(void* ptr)
        {
#if NULL_CHECKS
            if (ptr == null)
            {
                throw new NullReferenceException("Expected not-null"); ;
            }
#endif
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FileExists(string path) 
        {
#if FILE_PATH_CHECKS
            IsNotNull(path); // File.Exists only returns 'false' in case 'path' is null (no explicit throw, which is what I want)

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
#endif
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsWithinArrayBounds(int index, int arrayLength)
        {
#if BOUNDS_CHECKS
            if ((uint)index >= (uint)arrayLength)
            {
                throw new IndexOutOfRangeException($"{ index } out of range (length { arrayLength } - 1)");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsValidSubarray(int index, int numEntries, int arrayLength)
        {
#if SUBARRAY_CHECKS
            IsWithinArrayBounds(index, arrayLength);
            IsNotSmaller(numEntries, 0);

            if (index + numEntries > arrayLength)
            {
                throw new IndexOutOfRangeException($"index + numEntries is { index + numEntries }, which is larger than length { arrayLength }");
            }
#endif
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubarraysDoNotOverlap(int firstIndex, int secondIndex, int firstNumEntries, int secondNumEntries)
        {
#if SUBARRAY_CHECKS
            if (firstIndex < secondIndex)
            {
                if (firstIndex + firstNumEntries > secondIndex)
                {
                    throw new IndexOutOfRangeException($"Subarray from { firstIndex } to { firstIndex + firstNumEntries - 1} overlaps with subarray from { secondIndex } to { secondIndex + secondNumEntries - 1 }");
                }
            }
            else
            {
                if (secondIndex + secondNumEntries > firstIndex)
                {
                    throw new IndexOutOfRangeException($"Subarray from { secondIndex } to { secondIndex + secondNumEntries - 1} overlaps with subarray from { firstIndex } to { firstIndex + firstNumEntries - 1 }");
                }
            } 
#endif
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AreEqual<T>(T a, T b)
            where T : IEquatable<T>
        {
#if EQUALITY_CHECKS
            if (!a.Equals(b))
            {
                throw new ArgumentOutOfRangeException($"{ a } was expected to be equal to { b }");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AreNotEqual<T>(T a, T b)
            where T : IEquatable<T>
        {
#if EQUALITY_CHECKS
            if (a.Equals(b))
            {
                throw new ArgumentOutOfRangeException($"{ a } was expected not to be equal to { b }");
            }
#endif
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsBetween<T>(T value, T min, T max)
            where T : IComparable<T>
        {
#if COMPARE_CHECKS
            if ((value.CompareTo(min) < 0) || (0 < value.CompareTo(max)))
            {
                throw new ArgumentOutOfRangeException($"Min: { min }, Max: { max }, Value: { value }");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsSmallerOrEqual<T>(T value, T limit)
            where T : IComparable<T>
        {
#if COMPARE_CHECKS
            if (value.CompareTo(limit) == 1)
            {
                throw new ArgumentOutOfRangeException($"{ value } was expected to be smaller than or equal to { limit }");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsSmaller<T>(T value, T limit)
            where T : IComparable<T>
        {
#if COMPARE_CHECKS
            if (value.CompareTo(limit) != -1)
            {
                throw new ArgumentOutOfRangeException($"{ value } was expected to be smaller than { limit }");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotSmaller<T>(T value, T limit)
            where T : IComparable<T>
        {
#if COMPARE_CHECKS
            if (value.CompareTo(limit) == -1)
            {
                throw new ArgumentOutOfRangeException($"{ value } was expected not to be smaller than { limit }");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsGreaterOrEqual<T>(T value, T limit)
            where T : IComparable<T>
        {
#if COMPARE_CHECKS
            if (value.CompareTo(limit) == -1)
            {
                throw new ArgumentOutOfRangeException($"{ value } was expected to be greater than or equal to { limit }");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsGreater<T>(T value, T limit)
            where T : IComparable<T>
        {
#if COMPARE_CHECKS
            if (value.CompareTo(limit) != 1)
            {
                throw new ArgumentOutOfRangeException($"{ value } was expected to be greater than { limit }");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotGreater<T>(T value, T limit)
            where T : IComparable<T>
        {
#if COMPARE_CHECKS
            if (value.CompareTo(limit) == 1)
            {
                throw new ArgumentOutOfRangeException($"{ value } was expected not to be greater than { limit }");
            }
#endif
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsDefinedBitShift<T>(int amount)
            where T : unmanaged
        {
#if BIT_SHIFT_CHECKS
            if ((uint)amount >= (uint)sizeof(T) * 8u)
            {
                throw new ArgumentOutOfRangeException($"Shifting a { typeof(T) } by { amount } results in undefined behavior");
            }
#endif
        }
    }
}