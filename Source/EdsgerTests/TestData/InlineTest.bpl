procedure {:inline 3} incr(x: int) returns (r: int)
{
  r := x + 1;
}

procedure DivMod(a: int, b: int) returns (q: int, r: int)
requires (a >= 0 && b > 0);
ensures (a == q * b + r && 0 <= r && r < b);
{    
  q := 0;
  r := a;
    
  while (r >= b)
  invariant (a == q * b + r && 0 <= r);  
  {
    call q := incr(q);    
    r := r - b;
  }
}
