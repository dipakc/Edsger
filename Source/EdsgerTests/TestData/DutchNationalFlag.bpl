
procedure DutchNationalFlag(arr: [int]int, N: int) returns (r: int)
requires (0 <= N);
ensures ((exists p: int, q: int | 0 <= p <= q <= N :: (forall i: int | 0 <= i < p :: arr[i] == 0) && 
                                                      (forall i: int | p <= i < q :: arr[i] == 1) &&
                                                      (forall i: int | q <= i < N :: arr[i] == 2) );
{    

  
}
