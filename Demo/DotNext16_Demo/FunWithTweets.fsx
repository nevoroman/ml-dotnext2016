#load "references.fsx"

open System
open System.IO
open FSharp.Data
open FsPlot.Highcharts.Charting
 
// Prepearing data
let [<Literal>] tweetsPath = __SOURCE_DIRECTORY__ + "/data/fsharp_2013-2014.csv"
type Tweets = CsvProvider<tweetsPath>
let tweets = Tweets.GetSample()

// Let's get Top-20 twitter authors by tweets count

let top n (data:seq<_*seq<'a>>) = 
    data
    |> Seq.map(fun (name, s) -> name, (Seq.length s))
    |> Seq.sortByDescending(snd)
    |> Seq.take(n)

tweets.Rows 
    |> Seq.groupBy(fun x -> x.FromUserScreenName)
    |> top 20
    |> Chart.Pie 
    |> Chart.WithTitle "F# Top Twitter Authors"
    
// Ok, it's time to see, how most popular F# tweets words looks

let clearMessage(message:string) =     
    let words = message.ToLower().Split(' ') |> Array.filter(fun x -> x.StartsWith("@") |> not && x.StartsWith("http") |> not)
    String.Join(" ", words)
    |> String.filter(fun x -> not (x |> Char.IsPunctuation || x |> Char.IsDigit))

let stopwords = File.ReadAllLines(__SOURCE_DIRECTORY__ + "/data/stopwords")

let topWords = 
    tweets.Rows
    |> Seq.map(fun x -> clearMessage (x.Text))
    |> Seq.map(fun x -> x.Split(' '))
    |> Seq.concat
    |> Seq.filter(fun x -> x.Length > 2 && stopwords |> Array.contains(x) |> not)
    |> Seq.groupBy(fun x -> x)
    |> Seq.map(fun (x,y) -> x, Seq.length y)
    |> Seq.sortByDescending(snd)
    |> Seq.take(100)
    |> Seq.toArray
    
open RDotNet
open RProvider.wordcloud
open RProvider.RColorBrewer

let words = topWords |> Array.map(fst)
let freq = topWords |> Array.map(snd)

R.wordcloud(words, freq, random_order=false, colors=R.brewer_pal(8, "Dark2"))