namespace Domain

[<Measure>] type score

type BoardProcessor (board:Board) =

    let positionsByDigit (area:Set<Position>) : Map<int<sd>,Set<Position>> = 
        ``this is the place to start working``

    let housesByPosition =         
        // for each position on the board, get the house
        let housesByPosition' = 
            Index.AllPositions 
            |> Seq.map (fun pos -> 
                let house = new House(pos, board)
                (pos,house))
            |> Map.ofSeq
        lazy housesByPosition'

    let isBlank pos =
        not (board.At pos).IsSome

    let movesExist (house:House) : bool =
        not house.AvailableDigits.IsEmpty

    let isValid pos (house:House) : bool = 
        not (isBlank pos) ||
        (movesExist house)

    let to_pos (row:int) (column:int) (digit:int) = {Row=row*1<sr>;Col=column*1<sc>}

    member x.HousesByPosition = housesByPosition.Force ()

    member x.House (pos:Position) : House = 
        x.HousesByPosition.[pos]

    /// Board is valid if something can be played at every position
    member x.Validate () : bool =
        x.HousesByPosition |> Map.forall isValid

    /// Higher score = fewer choices
    member x.Score (position:Position) : int<score> =        
        let raw = 9 - (x.House position).AvailableDigits.Count
        // breakpoint = invalid board
        1<score> * raw

    member x.IsBlank (row:int) (column:int) (digit:int) =
        isBlank (to_pos row column digit)

    member x.CanPlay (row:int) (column:int) (digit:int) = 
        let pos = to_pos row column digit
        let blank = isBlank pos
        let house' = x.House pos
        let digitIsAvailable = 
            house'.AvailableDigits.Contains(digit*1<sd>)
        blank && digitIsAvailable

    override x.ToString() =
        Printer.PrintTemplate board
            (fun pos -> 
                let s = 
                    match board.At pos with
                    | Some x -> "->" + x.ToString() + "<-"
                    | None -> 
                        let house = x.HousesByPosition.[pos]
                        let sb = new System.Text.StringBuilder()
                        let chars = 
                            seq {
                                for x in house.AvailableDigits do
                                    sb.Append(x) |> ignore
                                    yield x
                            } |> Seq.toList
                        sb.ToString()
                s.PadRight(9)                   
            )
