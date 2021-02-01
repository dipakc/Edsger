// ensures ((exists p: int, q: int | 0 <= p && p <= q && q <= N :: (forall i: int | 0 <= i && i < p :: arr[i] == 0) && 
//                                                                 (forall i: int | p <= i && i < q :: arr[i] == 1) &&
//                                                                 (forall i: int | q <= i && i < N :: arr[i] == 2) );


var arr: [int]int;
var N: int;

procedure Swap(x: int, y: int)
modifies arr;
ensures arr[x] == old(arr)[y];
ensures arr[y] == old(arr)[x];
ensures (forall i: int :: i != x && i != y ==> arr[i] == old(arr)[i]);
{
    var tmp: int;
    tmp := arr[x];
    arr[x] := arr[y];
    arr[y] := tmp;
}

procedure DutchNationalFlag() returns (r: int, w: int)
modifies arr;
requires (0 <= N);
requires (forall i: int :: 0 <= i && i < N ==> arr[i] == 1 || arr[i] == 2 || arr[i] == 3);
ensures 0 <= r && r <= w && w <= N;
ensures (forall i: int :: 0 <= i && i < r ==> arr[i] == 1);
ensures (forall i: int :: r <= i && i < w ==> arr[i] == 2);
ensures (forall i: int :: w <= i && i < N ==> arr[i] == 3);
{    
    var b: int;
    r, w, b := 0, 0, N;
    
    while (w != b)
    invariant (forall i: int :: 0 <= i && i < r ==> arr[i] == 1);
    invariant (forall i: int :: r <= i && i < w ==> arr[i] == 2);
    invariant (forall i: int :: b <= i && i < N ==> arr[i] == 3);
    invariant 0 <= r && r <= w && w <= b && b <= N;
    {
        if (arr[w] == 1) {
            call Swap(w, r);
            r := r + 1;
            w := w + 1;
        } else if (arr[w] == 2){
            w := w + 1;
        } else if (arr[w] == 3){
            call Swap(w, b-1);
            b := b -1;
        }
    }
  
}
