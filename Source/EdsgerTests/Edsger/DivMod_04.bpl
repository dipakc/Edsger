function {:synthesize} f(r: int, a: int, b: int) returns (int);

procedure DivMod(a: int, b: int) returns (q: int, r: int)
requires (a >= 0 && b > 0);
ensures (a == q * b + r && 0 <= r && r < b);
{    

  r := a;
  q := 0;
  
  assert(a == q * b + r && 0 <= r);    
  
  while (r >= b)
  invariant (a == q * b + r && 0 <= r);  
  {
    q := q + 1;
    r := f(r, a, b);
    assert(a == q * b + r && 0 <= r);      
  }
  
  
}
