namespace EulerCore

open System.Numerics
open NumericLiteralI

module NumberTheory =

    let inline Divisors (n:^a) : ^a seq =
        let inline compute (n:^a) = 
            let zero:^a = LanguagePrimitives.GenericZero
            let one:^a = LanguagePrimitives.GenericOne

            let rec divisors (i:^a) = 
                seq {
                    if ((n % i) = zero) then 
                        //printf "(_%s) " (i.ToString())
                        yield i
                    let j:^a = i + one
                    if (j <= n / j) then yield! divisors j
                    if ( (i <> n / i) && (n % i = zero) ) then 
                        //printf "(^%s) " ((n/i).ToString())
                        yield n / i
                }
            divisors LanguagePrimitives.GenericOne
        compute n

    let inline ProperDivisors (n:^a) : ^a seq = 
        n |> Divisors |> Seq.filter (fun x -> x < n)

    let inline d (n:^a) = 
        Seq.fold (+) LanguagePrimitives.GenericZero (ProperDivisors n)

    let inline Amicable (a:^a,b:^a) = 
        (d(a) = b) &&
        (d(b) = a)

    let inline IsPerfect (a:^a) = (d(a) = a)
    let inline IsAbundant (a:^a) = (d(a) > a)
    let inline IsDeficient (a:^a) = (d(a) < a)
