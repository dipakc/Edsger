var arr: [int]int;

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