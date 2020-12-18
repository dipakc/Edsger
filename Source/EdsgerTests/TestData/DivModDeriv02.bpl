

procedure DivMod(a: int, b: int) returns (q: int, r: int)
requires (a >= 0 && b > 0);
ensures (a == q * b + r && 0 <= r && r < b);
{    

  assume(a == q * b + r && 0 <= r);    
  
  while (r >= b)
  invariant (a == q * b + r && 0 <= r);  
  {
    
  }
  
}
