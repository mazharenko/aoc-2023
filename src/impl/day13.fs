module impl.day13

let parse input : char[,][] =
    input
    |> Pattern2.read (Pattern1.read Seq.toArray >> array2D)
    
let private mirror matrix =
    Array2D.init (Array2D.length1 matrix) (Array2D.length2 matrix)
        (fun i j ->
            matrix[Array2D.length1 matrix - 1 - i, j])
    
let private dm matrix1 matrix2 =
   seq {
       for i in Array2D.rows matrix1 do
          for j in Array2D.cols matrix2 do
              if matrix1[i,j] <> matrix2[i,j]
              then yield 1
   } |> Seq.length
   
let private isMirrorAt matrix diff i =
    if i <= 0 then false
    elif i > Array2D.length1 matrix - 1 then false
    else
        let length = min i (Array2D.length1 matrix - i)
        let x = matrix[i - length .. i - 1, *]
        let y = matrix[i .. i + length - 1, *] |> mirror
        dm x y = diff
    
let private solvePattern targetDifference pattern = 
    let hReflectionLine =
        pattern
        |> Array2D.rows
        |> Array.tryFind (isMirrorAt pattern targetDifference)
    let transposed = Array2D.transpose pattern
    let vReflectionLine =
        transposed
        |> Array2D.rows
        |> Array.tryFind (isMirrorAt transposed targetDifference)
    (hReflectionLine |> Option.defaultValue 0) * 100
    +
    (vReflectionLine |> Option.defaultValue 0)
    
let solve1 (input : char[,][]) =
    input |> Array.sumBy (solvePattern 0)
    
let solve2 (input : char[,][]) =
    input |> Array.sumBy (solvePattern 1)