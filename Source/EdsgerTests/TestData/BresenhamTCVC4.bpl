function abs(int) returns (int);
axiom (forall x:int :: x >= 0 ==> abs(x) == x);
axiom (forall x:int :: x < 0 ==> abs(x) == -x);
 
/*
procedure Bresenham(A: int, B: int)
requires (0 <= B && B <= A);
{
    var x: int; var y: int; var e: int;
    
    e := 2 * B - A;
    y := 0;
    x := 0;
    
    while (x <= A)
    invariant e == 2 * (x + 1) * B - (2 * y + 1) * A;
    invariant 2 * (B - A) <= e && e <= 2 * B;
    {
        assert 2 * abs(A * y - B * x) <= A;
        
        if (e < 0) 
        {
            e := e + 2 * B;
        }
        else
        {
            e := e + 2 * (B - A);
            y := y + 1;
        }
        x := x + 1;
    }    
}
*/


procedure Bresenham(A: int, B: int)
requires (0 <= B && B <= A);
{
    var x: int; var y: int; var e: int;
    
    e := 2 * B - A;
    y := 0;
    x := 0;
    
    while (x <= A)
    invariant e == 2 * (x + 1) * B - (2 * y + 1) * A;
    invariant 2 * (B - A) <= e && e <= 2 * B;
    invariant 2 * abs(A * y - B * x) <= A;
    {
        if (e < 0) 
        {
            e := e + 2 * B;
            assert e == 2 * (x + 2) * B - (2 * y + 1) * A;
            assert 2 * (B - A) <= e && e <= 2 * B;

        }
        else
        {
            e := e + 2 * (B - A);
            y := y + 1;
            assert e == 2 * (x + 2) * B - (2 * y + 1) * A;
            assert 2 * (B - A) <= e && e <= 2 * B;            
        }
        x := x + 1;
    }    
}