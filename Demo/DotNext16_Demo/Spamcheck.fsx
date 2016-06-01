#load "references.fsx"

open System
open System.IO
open FSharp.Data
open System.Text.RegularExpressions
open Accord.MachineLearning
open Accord.MachineLearning.Bayes

let [<Literal>] smsPath = __SOURCE_DIRECTORY__ + "/data/SMSSpamCollection"
type SMSCorpus = CsvProvider<smsPath>
let sms = SMSCorpus.GetSample()

let labelToVal label = 
    match label with
    | "ham" -> 0
    | "spam" -> 1
    | _ -> failwith "Unknown label"
        
let messages = sms.Rows |> Seq.map(fun x -> (labelToVal x.Label, x.Text)) |> Seq.toArray
let validation, training = messages.[..999], messages.[1000..]

let tokenize (text:string) = 
    text.ToLower()
    |> Regex(@"\w+").Matches
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.Value)
    |> Seq.toArray

let evaluate (tokens:string[]) = 
    let bow = BagOfWords(tokens)
    let inputs = training |> Seq.map(fun (_,text) -> bow.GetFeatureVector (tokenize text)) |> Seq.toArray
    let outputs = training |> Array.map fst
    let symbols:int[] = Array.init (bow.NumberOfWords) (fun x -> 2)
    let bayes = NaiveBayes(2, symbols)
    bayes.Estimate(inputs, outputs) |> ignore
    
    validation |> Seq.averageBy(fun (label,text) ->
        let result = bayes.Compute(bow.GetFeatureVector (tokenize text))
        if (result = label) then 1. else 0.
    )

// Trying to train by all words
let allWords = 
    training 
    |> Array.map(fun (_,text) -> tokenize text) 
    |> Array.concat

evaluate allWords

let hamCount = messages |> Seq.filter(fun (label,_) -> label = 0) |> Seq.length
let totalCount = messages |> Seq.length

float hamCount / float totalCount

let ham,spam = 
    let rawHam,rawSpam =
        training 
        |> Array.partition (fun (lbl,_) -> lbl=0)
    rawHam |> Array.map snd, 
    rawSpam |> Array.map snd

let top n (messages:string[]) = 
    messages
    |> Array.map(tokenize)
    |> Array.concat
    |> Array.groupBy(fun x -> x)
    |> Array.map(fun (word,words) -> word,Seq.length words)
    |> Array.sortByDescending(snd)
    |> Array.map(fst)
    |> Array.take(n)
    

let hamWordsCount = ham |> Array.map(tokenize) |> Array.concat |> Array.groupBy(fun x -> x) |> Array.length
let spamWordsCount = spam |> Array.map(tokenize) |> Array.concat |> Array.groupBy(fun x -> x) |> Array.length

let hamLength = int (float hamWordsCount * (8./100.))
let spamLength = int (float spamWordsCount * (8./100.))

let topSpam =  top spamLength spam
let topHam =  top hamLength ham
    
let topTokens = Array.append topSpam topHam

evaluate topTokens