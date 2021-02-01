function {:inline 5} abs(x: int): int
{
   if x < 0 then -x else x
}
 
function {:synthesize} f(B: int, e: int) returns (int);

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
        // Mark the point.
        //assert 2 * abs(A * y - B * x) <= A;
        
        assume e < 0; 
        
        //e := e + 2 * B;
        e := f(B, e);
        assert 2 * abs(A * y - B * (x + 1)) <= A;            
        
        x := x + 1;
        //assert 2 * abs(A * y - B * x) <= A;
    } 
}

// /Users/dipakc/progs/cvc4_1_8/cvc4_wrapper.sh --lang=sygus2 --no-strict-parsing --no-condense-function-values --produce-models /Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestOutput/_BresenhamCopy.sygus
