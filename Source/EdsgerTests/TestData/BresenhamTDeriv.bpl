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
/*


/*
procedure Bresenham(A: int, B: int)
requires (0 <= B && B <= A);
{
    var x: int; var y: int; var e: int;
    
    e := 2 * B - A;
    y := 0;
    x := 0;
    
    while (x <= A)    
    {
        assume 2 * abs(A * y - B * x) <= A;
        //draw(x, y)
                                
        x := x + 1;
    }    
}
*/


/*
procedure Bresenham(A: int, B: int)
requires (0 <= B && B <= A);
{
    var x: int; var y: int; var e: int;
    
    e := 2 * B - A;
    y := 0;
    x := 0;
    
    while (x <= A)
    invariant 2 * abs(A * y - B * x) <= A;    
    {
        assert 2 * abs(A * y - B * x) <= A;
        //draw(x, y)
                                
        x := x + 1;
        assume 2 * abs(A * y - B * x) <= A;
    }    
}
*/

/*
procedure Bresenham(A: int, B: int)
requires (0 <= B && B <= A);
{
    var x: int; var y: int; var e: int;
    
    e := 2 * B - A;
    y := 0;
    x := 0;
    
    while (x <= A)
    invariant 2 * abs(A * y - B * x) <= A;    
    {
        assert 2 * abs(A * y - B * x) <= A;
        //draw(x, y)
        goto A, B;
        A:                                        
            y := y + 1; 
            assume 2 * abs(A * y - B * (x + 1)) <= A;
            goto E; 
        B:
            assume 2 * abs(A * y - B * (x + 1)) <= A;
            goto E; 
        E:            
        assume 2 * abs(A * yt - B * (x + 1)) <= A;
        x := x + 1;       
    }    
}

*/

/*
procedure Bresenham(A: int, B: int)
requires (0 <= B && B <= A);
{
    var x: int; var y: int; var e: int;
    
    e := 2 * B - A;
    y := 0;
    x := 0;
    
    while (x <= A)
    invariant 2 * abs(A * y - B * x) <= A;    
    {
        assert 2 * abs(A * y - B * x) <= A;
        //draw(x, y)
        goto A, B;
        A:                                        
            y := y + 1;             
            assume 2 * abs(A * y - B * (x + 1)) <= A;
            goto E; 
        B:
            assume 2 * abs(A * y - B * (x + 1)) <= A;
            goto E; 
        E:            
        assert 2 * abs(A * y  - B * (x + 1)) <= A;
        x := x + 1;       
    }    
}
*/

/*
procedure Bresenham(A: int, B: int)
requires (0 <= B && B <= A);
{
    var x: int; var y: int; var e: int;
    
    e := 2 * B - A;
    y := 0;
    x := 0;
    
    while (x <= A)
    invariant 2 * abs(A * y - B * x) <= A;    
    {
        assert 2 * abs(A * y - B * x) <= A;
        //draw(x, y)
        goto A, B;
        A:                                        
            assume 2 * abs(A * (y + 1) - B * (x + 1)) <= A;
            y := y + 1;             
            goto E; 
        B:
            assume 2 * abs(A * y - B * (x + 1)) <= A;
            goto E; 
        E:            
        assert 2 * abs(A * y  - B * (x + 1)) <= A;
        x := x + 1;       
    }    
}
*/

/*
    2 * abs(A * (y + 1) - B * (x + 1)) <= A;
 === {assume A * (y + 1) - B * (x + 1) >= 0 }
    2 A (y + 1) - 2B(x+1) <= A
 ===
     2(x+1)B - 2(y+1)A >= 0
 === {Let e = 2(x+1)B - (2y+1)A >= 0}
     e >= 0
*/
               
    
procedure Bresenham(A: int, B: int)
requires (0 <= B && B <= A);
{
    var x: int; var y: int; var e: int;
    
    e := 2 * B - A;
    y := 0;
    x := 0;
    
    while (x <= A)
    invariant 2 * abs(A * y - B * x) <= A;
    invariant e == 2 * (x + 1) * B - ( 2 * y + 1) A;        
    {
        assert 2 * abs(A * y - B * x) <= A;
        //draw(x, y)
        goto A, B;
        A:                                        
            assume e >= 0
            y := y + 1;             
            goto E; 
        B:
            assume 2 * abs(A * y - B * (x + 1)) <= A;
            goto E; 
        E:            
        assert 2 * abs(A * y  - B * (x + 1)) <= A;
        x := x + 1;       
    }    
}
