namespace ProblemEighteen

open System.Collections.Generic

type Triangle(levels:int [,]) =

    let _scores:(int [,]) = Array2D.zeroCreate levels.Length levels.Length

    member self.Value(level, offset) = 
        levels.[level, offset]
    
    member self.GetScore(level, offset) = 
        match (level, offset) with
        | (a,b) when a<0 || b<0 -> None
        | (a,b) when a>=self.Height || b>a -> None
        | (a,b) -> Some _scores.[level, offset]

    member self.SetScore (level, offset) value = 
        _scores.[level, offset] <- value

    member self.Height = 
        Array2D.length1 levels

    member self.Max a b = 
        match (a, b) with
        | (None, Some x) -> x
        | (Some x, None) -> x
        | (None, None) -> 0
        | (Some x, Some y) ->
            if (x>y) then x else y

    member self.Solve = 
        for level = (self.Height-1) downto 0 do
            for offset = 0 to level do
                let downAndLeftScore = self.GetScore(level+1, offset)
                let downAndRightScore = self.GetScore(level+1, offset+1)
                self.SetScore(level, offset) 
                    (self.Value(level, offset) + 
                       (self.Max downAndLeftScore downAndRightScore) )
        _scores.[0,0]
