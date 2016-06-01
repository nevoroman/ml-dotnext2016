#load "references.fsx"

open System
open System.IO
open FSharp.Data

open java.util
open edu.stanford.nlp.ling
open edu.stanford.nlp.pipeline
open edu.stanford.nlp.trees
open edu.stanford.nlp.sentiment
open edu.stanford.nlp.neural.rnn

let srcDirectory = __SOURCE_DIRECTORY__
let jarDirectory =
    __SOURCE_DIRECTORY__
    + @"/stanford-corenlp/models/"
    
let props = Properties()
props.setProperty("annotators",
    "tokenize, ssplit, pos, parse, sentiment") |> ignore
props.setProperty("sutime.binders","0") |> ignore
let tree = SentimentCoreAnnotations.SentimentAnnotatedTree().getClass()
let sentence = CoreAnnotations.SentencesAnnotation().getClass()

Directory.SetCurrentDirectory(jarDirectory)
let pipeline = StanfordCoreNLP(props)
Directory.SetCurrentDirectory(srcDirectory)

let getSentimentMeaning value =
    match value with
    | 0 -> "Negative"
    | 1 -> "Somewhat negative"
    | 2 -> "Neutral"
    | 3 -> "Somewhat positive"
    | 4 -> "Positive"
    | _ -> failwith "Wrong sentiment value"

let evaluateSentiment (text:string) =
    let annotation = Annotation(text)
    pipeline.annotate(annotation)

    let sentences = annotation.get(sentence) :?> java.util.ArrayList

    let sentiments =
        [ for s in sentences ->
            let sentence = s :?> Annotation
            let sentenceTree = sentence.get(tree) :?> Tree
            let sentiment = RNNCoreAnnotations.getPredictedClass(sentenceTree)
            let preds = RNNCoreAnnotations.getPredictions(sentenceTree)
            let probs = [ for i in 0..4 -> preds.get(i)]
            sentiment, probs ]
    fst sentiments.[0]

let printSentiment text = 
    let sentiment = evaluateSentiment text
    printf "\"%s\" - %s\n" text (getSentimentMeaning sentiment) 


let [<Literal>] tweetsPath = __SOURCE_DIRECTORY__ + "/data/fsharp_2013-2014.csv"
type Tweets = CsvProvider<tweetsPath>

let shuffle (r : Random) xs = xs |> Seq.sortBy (fun _ -> r.nextInt())
let takeSomethingPositive() = 
    Tweets.GetSample().Rows 
    |> shuffle(Random()) 
    |> Seq.filter(fun x -> (evaluateSentiment x.Text) > 2 &&  x.Text.Contains("RT") |> not )
    |> Seq.head

printf "%s\n" (takeSomethingPositive().Text)