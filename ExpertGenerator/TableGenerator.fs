module ExpertGenerator.TableGenerator

open XmlModels

type ResultItem =
    { Id: string
      Name: string
      Value: string }

let getResults (diagram: Tree) =
    let results = ResizeArray()

    for node in diagram do
        match node.Value with
        | :? Result as result -> results.Add(result)
        | _ -> ()

    results.ToArray()

let getVariables (diagram: Tree) =
    let results = ResizeArray<ResultItem>()
    let leaves = ResizeArray<Result>()
    let mutable questions = Map<string, string> []

    for node in diagram do
        match node.Value with
        | :? Result as result -> leaves.Add(result)
        | :? Question as question ->
            match questions.TryFind question.Text with
            | Some s -> question.VarName <- s
            | None ->
                question.VarName <- $"Variable{question.Id}"
                questions <- questions.Add(question.Text, question.VarName)

                results.Add(
                    { Id = question.Id.ToString()
                      Name = question.VarName
                      Value = question.Text }
                )

    let from = leaves |> Seq.head |> _.Id
    let to' = leaves |> Seq.last |> _.Id

    results.Add
        { Id = $"{from}-{to'}"
          Name = "Rez"
          Value = leaves |> Seq.map _.Name |> Set.ofSeq |> String.concat ", " }

    results.ToArray()

type Condition = { VarName: string; Value: string }

type KnowledgeItem =
    { Number: int
      Conditions: Condition list
      Result: string
      Path: string }
    
    member this.ConditionToString() =
        let conditions = 
            this.Conditions
            |> List.map (fun x -> $"{x.VarName} = \"{x.Value}\"")
            |> String.concat " И "
        
        $"ЕСЛИ {conditions} ТО Rez = \"{this.Result}\""
    member this.ToTuple() =              
        (this.Number, this.ConditionToString(), this.Path)
    override this.ToString() =               
        $"{this.Number};{this.ConditionToString()};{this.Path}"

let rec traverseTree (currentPath: (string * string) list) (currentIds: int list) (node: Node) : KnowledgeItem list =   
    match node with
    | :? Result as result ->
        let id = result.Id
        let name = result.Name
        let conditions =
            currentPath
            |> List.rev
            |> List.map (fun (varName, value) -> { VarName = varName; Value = value })
        let path = (id :: currentIds) |> List.map string |> String.concat ","       
        [ { Number = 0
            Conditions = conditions
            Result = name
            Path = path } ]
    | :? Question as question ->
        let id = question.Id
        let varName = question.VarName
        let children = question.Children
        children
        |> Seq.map (fun x -> x.Key, x.Value)
        |> Seq.toList
        |> List.collect (fun (value, childNode) ->
            traverseTree ((varName, value) :: currentPath) (id :: currentIds) childNode )
    | _ -> failwith "Unknown node type"
let generateKnowledgeBase (root: Node) =
    traverseTree [] [] root
    |> List.mapi (fun i x -> { x with Number = i + 1 })
