function abs(int) returns (int);
axiom (forall x:int :: x >= 0 ==> abs(x) == x);
axiom (forall x:int :: x < 0 ==> abs(x) == -x);
 
/*
procedure Bresenham(A: int, B: int) returns (out: [int]int)
requires (0 < B && B <= A);
ensures (forall k: int :: 0 <= k && k <= A ==> 2 * abs(A * out[k] - B * k) <= A);
{
    assume (forall k: int :: 0 <= k && k <= A ==> 2 * abs(A * out[k] - B * k) <= A);
}
*/

/*
procedure Bresenham(A: int, B: int) returns (out: [int]int)
requires (0 < B && B <= A);
ensures (forall k: int :: 0 <= k && k <= A ==> 2 * abs(A * out[k] - B * k) <= A);
{
    var x: int;
    
    assume (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A); 
    assume x == A + 1; // implies A < x
}
*/


/*
procedure Bresenham(A: int, B: int) returns (out: [int]int)
requires (0 < B && B <= A);
ensures (forall k: int :: 0 <= k && k <= A ==> 2 * abs(A * out[k] - B * k) <= A);
{
    var x: int;
    
    assume (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A);
    
    while (x != A + 1)
    invariant (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A);
    {
    
    }
        
    assert (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A); 
    assert x == A + 1; // implies A < x
}
*/

/*
procedure Bresenham(A: int, B: int) returns (out: [int]int)
requires (0 < B && B <= A);
ensures (forall k: int :: 0 <= k && k <= A ==> 2 * abs(A * out[k] - B * k) <= A);
{
    var x: int;
    
    x := 0;
    
    while (x != A + 1)
    invariant (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A);
    {
    
    }
        
    assert (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A); 
    assert x == A + 1; // implies A < x
}

*/


/*
procedure Bresenham(A: int, B: int) returns (out: [int]int)
requires (0 < B && B <= A);
ensures (forall k: int :: 0 <= k && k <= A ==> 2 * abs(A * out[k] - B * k) <= A);
{
    var x: int;
    
    x := 0;
    
    while (x != A + 1)
    invariant (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A);
    {
        
        assume (forall k: int :: 0 <= k && k < x + 1 ==> 2 * abs(A * out[k] - B * k) <= A);
        x := x + 1;
    }
        
    assert (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A); 
    assert x == A + 1; // implies A < x
}
*/

/*
procedure Bresenham(A: int, B: int) returns (out: [int]int)
requires (0 < B && B <= A);
ensures (forall k: int :: 0 <= k && k <= A ==> 2 * abs(A * out[k] - B * k) <= A);
{
    var x: int;
    
    x := 0;
    
    while (x != A + 1)
    invariant (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A);
    {
        
        assume (2 * abs(A * out[x] - B * x) <= A);
        x := x + 1;
    }
        
    assert (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A); 
    assert x == A + 1; // implies A < x
}
*/

procedure Bresenham(A: int, B: int) returns (out: [int]int)
requires (0 < B && B <= A);
ensures (forall k: int :: 0 <= k && k <= A ==> 2 * abs(A * out[k] - B * k) <= A);
{
    var x: int;
    
    x := 0;
    
    while (x != A + 1)
    invariant (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A);
    {
        
        assume (2 * abs(A * out[x] - B * x) <= A);
        x := x + 1;
    }
        
    assert (forall k: int :: 0 <= k && k < x ==> 2 * abs(A * out[k] - B * k) <= A); 
    assert x == A + 1; // implies A < x
}


//    invariant v == 2 * (x + 1) * B - (2 * y + 1) * A;
//    invariant 2 * (B - A) <= v && v <= 2 * B;
