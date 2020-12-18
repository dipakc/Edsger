function abs(int) returns (int);
axiom (forall x:int :: x >= 0 ==> abs(x) == x);
axiom (forall x:int :: x < 0 ==> abs(x) == -x);
 

// BoogieZ3 is not able to verify this.

procedure Bresenham(A: int, B: int)
requires (0 < B && B <= A);
{
    var x: int; var y: int; var v: int;
    
    v := 2 * B - A;
    y := 0;
    x := 0;
    
    while (x <= A)
    invariant 0 < B && B <= A;
    invariant v == 2 * (x + 1) * B - (2 * y + 1) * A;
    invariant 2 * (B - A) <= v && v <= 2 * B;
    invariant (forall k:int :: 0 <= k && k < x ==> 2 * abs(A * y - B * k) <= A);
    {
        // assert 2 * abs(A * y - B * x) <= A;
        if (v < 0) 
        {
            v := v + 2 * B;
            assert (forall k:int :: 0 <= k && k < x + 1 ==> 2 * abs(A * y - B * k) <= A);
            x := x + 1;
        }
        else
        {
            v := v + 2 * (B - A);
            assert (forall k:int :: 0 <= k && k < x + 1 ==> 2 * abs(A * (y + 1) - B * k) <= A);
            x := x + 1;            
            y := y + 1;
        }
    }    
}
