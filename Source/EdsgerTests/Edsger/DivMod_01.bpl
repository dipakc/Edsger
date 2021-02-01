
procedure DivMod(a: int, b: int) returns (q: int, r: int);
requires a >= 0 && b > 0;
ensures a == q * b + r && 0 <= r && r < b;

implementation DivMod(a: int, b: int) returns (q: int, r: int)
{
    assume a == q * b + r && 0 <= r && r < b;
}