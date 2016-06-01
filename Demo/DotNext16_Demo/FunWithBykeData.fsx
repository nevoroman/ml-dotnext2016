#load "references.fsx"

open System
open System.IO
open FSharp.Data
open FSharp.Charting
open Accord.Statistics.Models.Regression.Linear

let [<Literal>] dataPath = __SOURCE_DIRECTORY__ + "/data/day.csv"

type Data = CsvProvider<dataPath>
let dataset = Data.GetSample()
let data = dataset.Rows |> Seq.toArray

let shuffle (r : Random) xs = xs |> Array.sortBy (fun _ -> r.Next())
let randomedData = data |> shuffle(Random(10))
let training,validation =  randomedData.[..600],randomedData.[600..]


let count =  data |> Seq.map (fun x -> float x.Cnt)
Chart.Line count


let boolToFloat value = 
        match value with
        | true -> 1.
        | false -> 0.
let toShortInputLine (day:Data.Row) = 
    [| boolToFloat day.Holiday; float day.Season; float day.Windspeed; float day.Mnth |]

let simpleRegression = new MultipleLinearRegression(4, true);
let shortInputs = training |> Array.map(toShortInputLine)
let outputs = training |> Array.map(fun x -> float x.Cnt)

let error = simpleRegression.Regress(shortInputs, outputs)

let validationInputs = validation |> Array.map(toShortInputLine)
let predictedCount = simpleRegression.Compute(validationInputs)
let realCount = validation |> Array.map(fun x -> float x.Cnt)

Chart.Combine [Chart.Line realCount; Chart.Line predictedCount]

    


let toInputLine (day:Data.Row) = 
    [| float day.Atemp; float day.Casual; boolToFloat day.Holiday; float day.Hum; float day.Instant; 
       float day.Mnth; float day.Registered; float day.Season; float day.Temp; float day.Weathersit; 
       float day.Weekday; float day.Windspeed; boolToFloat day.Workingday; boolToFloat day.Yr; |]
    
let regression = new MultipleLinearRegression(14, true);
let inputs = training |> Seq.map(toInputLine) |> Seq.toArray

let errorNew = regression.Regress(inputs, outputs)

let predictedCountNew = regression.Compute(validation |> Array.map(toInputLine))
let realCountNew = validation |> Array.map(fun x -> float x.Cnt)


Chart.Combine [Chart.Line realCountNew; Chart.Line predictedCountNew]
