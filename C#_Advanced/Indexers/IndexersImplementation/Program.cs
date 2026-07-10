// An indexer in C# is a structural member that allows instances of a class, struct, or interface to be accessed using array-like square bracket syntax [].
// Often called smart arrays or parameterized properties
// indexers let you treat an entire object as a virtual collection without exposing its underlying implementation or data structures
var IP = new IP(199, 200, 211, 204);
Console.WriteLine(IP[2]);

public class IP
{
    private int[] _Segments = new int[4];

    public IP(int seg1, int seg2, int seg3, int seg4)
    {
        _Segments[0] = seg1;
        _Segments[1] = seg2;
        _Segments[2] = seg3;
        _Segments[3] = seg4;
    }

    public int this[int ind]
    {
        get
        {
            if (ind >= _Segments.Length)
                throw new ArgumentOutOfRangeException("The index must be with in range [0-3]");
            else
            {
                return _Segments[ind];
            }
        }
        set
        {
            if (ind >= _Segments.Length)
                throw new ArgumentOutOfRangeException("The index must be with in range [0-3]");
            else
                _Segments[ind] = value;
        }
    }

    public string GetIp => string.Join(".", _Segments);
}
