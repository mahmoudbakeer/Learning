using System;
using System.Collections;

// ==========================================
// 28. BITARRAY (System.Collections)
// Used to manage a compact array of bit values (booleans).
// It is highly memory-efficient because it stores actual bits, not full bytes!
// ==========================================

class Program
{
    // Your highly optimized custom method to visualize the bits
    static string BitArrayToString(BitArray bitarr)
    {
        char[] cArr = new char[bitarr.Length];
        for (int i = 0; i < bitarr.Length; i++)
            cArr[i] = bitarr[i] ? '1' : '0';
        return new string(cArr);
    }

    static void Main()
    {
        // ==========================================
        // A. WAYS TO DECLARE AND INITIALIZE BITARRAY
        // ==========================================

        // 1. By Length (Defaults to all 'false' / 0)
        BitArray arr1 = new BitArray(10);

        // 2. By Length and Default Value (All set to 'true' / 1)
        BitArray arr2 = new BitArray(10, true);

        // 3. From a Boolean Array
        bool[] bools = [true, false, true];
        BitArray arr3 = new BitArray(bools);

        // 4. From a Byte Array (Each byte becomes 8 bits!)
        byte[] bytes = [1]; // 1 in binary is 00000001
        BitArray arr4 = new BitArray(bytes);

        // 5. From an Integer Array (Each int becomes 32 bits!)
        int[] ints = [5]; // 5 in binary is 101
        BitArray arr5 = new BitArray(ints);


        // ==========================================
        // B. WORKING WITH BITARRAY
        // ==========================================
        BitArray bitArr = new BitArray(10);

        // Changing specific elements
        bitArr.Set(2, true);
        bitArr.Set(9, true);
        bitArr.Set(1, true);
        bitArr.Set(4, true);
        bitArr.Set(6, true);

        Console.WriteLine($"First Array Size: {bitArr.Count}");
        Console.WriteLine($"First Array Bits: {BitArrayToString(bitArr)}\n");

        BitArray secondBitArray = new BitArray(10);
        secondBitArray.Set(2, true);
        secondBitArray.Set(7, true);
        secondBitArray.Set(1, true);
        secondBitArray.Set(3, true);
        secondBitArray.Set(5, true);

        Console.WriteLine($"Second Array Bits: {BitArrayToString(secondBitArray)}\n");


        // ==========================================
        // C. BITWISE OPERATIONS (AND, OR, XOR, NOT)
        // 🚨 WARNING: These methods MODIFY the original array IN-PLACE!
        // To avoid data corruption in sequential operations, clone them first.
        // ==========================================

        Console.WriteLine("--- Bitwise Operations ---");

        // 1. AND Operation
        // We clone secondBitArray so we don't destroy its original value
        BitArray cloneForAnd = new BitArray(secondBitArray);
        var resultAnd = cloneForAnd.And(bitArr);
        Console.WriteLine($"Result of AND: {BitArrayToString(resultAnd)}");

        // 2. OR Operation
        BitArray cloneForOr = new BitArray(secondBitArray);
        var resultOr = cloneForOr.Or(bitArr);
        Console.WriteLine($"Result of OR : {BitArrayToString(resultOr)}");

        // 3. XOR Operation
        BitArray cloneForXor = new BitArray(secondBitArray);
        var resultXor = cloneForXor.Xor(bitArr);
        Console.WriteLine($"Result of XOR: {BitArrayToString(resultXor)}");

        // 4. NOT Operation (Reverses all bits: 1 becomes 0, 0 becomes 1)
        BitArray cloneForNot = new BitArray(bitArr);
        Console.WriteLine($"Result of NOT (on First Array): {BitArrayToString(cloneForNot.Not())}");
    }
}
