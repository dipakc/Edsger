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
    var b: int;
    r, w, b := 0, 0, N;

    assert 0 <= r && r <= w && w <= b && b <= N;
    assert color(arr, 0, r, 1);
    assert color(arr, r, w, 2);
    assert color(arr, b, N, 3);

    while (w != b)
    invariant 0 <= r && r <= w && w <= b && b <= N;
    invariant color(arr, 0, r, 1);
    invariant color(arr, r, w, 2);
    invariant color(arr, b, N, 3);
    {
        if (arr[w] == 1) {
            assume false;
        } else if (arr[w] == 2){
            assume false;
        } else if (arr[w] == 3){
            assume false;
        }
    }
    assert 0 <= r && r <= w && w <= b && b <= N;
    assert color(arr, 0, r, 1);
    assert color(arr, r, w, 2);
    assert color(arr, b, N, 3);
    assert w == b;
}
