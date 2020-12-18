function abs(int) returns (int);
axiom (forall x:int :: x >= 0 ==> abs(x) == x);
axiom (forall x:int :: x < 0 ==> abs(x) == -x);
 

procedure Bresenham(A: int, B: int) returns (out: [int]int)
requires (0 < B && B <= A);
ensures (forall k: int :: 0 <= k && k <= A ==> 2 * abs(A * out[k] - B * k) <= A);
{
    var x: int; var y: int; var v: int;
    
    v := 2 * B - A;
    y := 0;
    x := 0;
    
    while (x <= A)
    invariant 0 < B && B <= A;
    invariant v == 2 * (x + 1) * B - (2 * y + 1) * A;
    invariant 2 * (B - A) <= v && v <= 2 * B;
    invariant (forall k:int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A);
    {
        if (v < 0) 
        {
            out[x] := y;
            v := v + 2 * B;
            x := x + 1;
        }
        else
        {
            out[x] := y;
            v := v + 2 * (B - A);
            x := x + 1;
            y := y + 1;
        }
    }    
}
