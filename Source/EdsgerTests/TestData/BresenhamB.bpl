function abs(int) returns (int);
axiom (forall x:int :: x >= 0 ==> abs(x) == x);
axiom (forall x:int :: x < 0 ==> abs(x) == -x);
 

procedure BresenhamB(A: int, B: int)
requires (0 < B && B <= A);
// ensures (forall k: int :: 0 <= k && k <= A ==> 2 * abs(A * out[k] - B * k) <= A);
{
    var x: int; var y: int; var v: int; 

    assume 0 < B && B <= A;
    assume v == 2 * (x + 1) * B - (2 * y + 1) * A;
    assume 2 * (B - A) <= v && v <= 2 * B;
    assume 2 * abs(A * y - B * x) <= A;
    assume 0 <= x && x < A;
        
    if (v < 0) 
    {
        assert v < 0;
        v := v + 2 * B;
        x := x + 1;
        assert v == 2 * (x + 1) * B - (2 * y + 1) * A;
        assert 2 * (B - A) <= v && v <= 2 * B;
        assert 2 * abs(A * y - B * x) <= A;

    }
    else
    {
        assert v >= 0;
        v := v + 2 * (B - A);
        x := x + 1;
        y := y + 1;
        assert v == 2 * (x + 1) * B - (2 * y + 1) * A;
        assert 2 * (B - A) <= v && v <= 2 * B;
        assert 2 * abs(A * y - B * x) <= A;        
    }

    assert v == 2 * (x + 1) * B - (2 * y + 1) * A;
    assert 2 * (B - A) <= v && v <= 2 * B;
    assert 2 * abs(A * y - B * x) <= A;
        
}
