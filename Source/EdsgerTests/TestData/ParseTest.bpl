function {:hole} f(r: int, a: int, b: int) returns (int);

procedure DivMod(a: int, b: int) returns (q: int, r: int)
requires (a >= 0 && b > 0);
ensures (a == q * b + r && 0 <= r && r < b);
{    

  q := 0;
  r := a;

block1:      
  while (r >= b)
  invariant (a == q * b + r && 0 <= r);  
  {
    q := q + 1;    
    //r := r - b;
    r := f(r, a, b);

    while (r >= r)
    invariant (a == q * b + r && 0 <= r);  
    {
        q := q + 1;    
        //r := r - b;
        r := f(r, a, b);    
    }
  }
  q := 2;
  r := 2;

block2:
  while (b <= r)
  invariant (a == q * b + r && 0 <= r);  
  {
    q := q + 2;    
    r := r + 2;    
  }

  q := 3;
  r := 3;
  
}
