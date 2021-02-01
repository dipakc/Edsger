
var arr: [int]int;

procedure SimpleArray()
modifies arr;
ensures (arr[0] == 2);
{    
    arr[0] := 2;  
}
