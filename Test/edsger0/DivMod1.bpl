// RUN: %edsger /errorTrace:0 /nologo \ 
// RUN: /proverOpt:SOLVER=CVC4 \
// RUN: /proverLog:%t.@PROC@.sygus \
// RUN: /synth \
// RUN: /synthGrammar:%S/../grammar.txt %s > "%t"

// RUN: %diff "%s.expect" "%t"

function {:hole} f(r: int, a: int, b: int) returns (int);

procedure DivMod(a: int, b: int) returns (q: int, r: int)
requires (a >= 0 && b > 0);
ensures (a == q * b + r && 0 <= r && r < b);
{    
  q := 0;
  r := a;
    
  while (r >= b)
  invariant (a == q * b + r && 0 <= r);  
  {
    q := q + 1;    
    //r := r - b;
    r := f(r, a, b);
  }
}

