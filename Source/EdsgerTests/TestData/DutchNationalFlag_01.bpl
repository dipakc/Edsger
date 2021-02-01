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

function color(arr: [int]int, p: int, q: int, c: int) returns (bool)  {
    (forall i: int :: p <= i && i < q ==> arr[i] == c)
}

procedure DutchNationalFlag() returns (r: int, w: int)
modifies arr;
requires (0 <= N);
requires (forall i: int :: 0 <= i && i < N ==> arr[i] == 1 || arr[i] == 2 || arr[i] == 3);
ensures 0 <= r && r <= w && w <= N;
ensures color(arr, 0, r, 1);
ensures color(arr, r, w, 2);
ensures color(arr, w, N, 3);    
{    

    assume 0 <= r && r <= w && w <= N;
    assume color(arr, 0, r, 1);
    assume color(arr, r, w, 2);
    assume color(arr, w, N, 3);
    
}
