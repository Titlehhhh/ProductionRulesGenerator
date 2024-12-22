module ExpertGenerator.TableGenerator

open XmlModels

type ResultItem =
    { Id: string
      Name: string
      Value: string }

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


let rec traverseTree
    (currentPath: (string * string) list)
    (currentIds: int list)
    (node: Node)
    : (int * string * string) list =
    match node with
    | :? Result as result ->
        let id = result.Id
        let name = result.Name

        let conditions =
            currentPath
            |> List.map (fun (varName, value) -> $"{varName} = \"{value}\"")
            |> String.concat " И "

        let fullCondition = $"ЕСЛИ {conditions} ТО Результат = {name}"
        let path = (id :: currentIds) |> List.rev |> List.map string |> String.concat ","
        [ (0, fullCondition, path) ]
    | :? Question as question ->
        let id = question.Id
        let varName = question.VarName
        let children = question.Children

        children
        |> Seq.map (fun x -> x.Key, x.Value)
        |> Seq.toList
        |> List.collect (fun (value, childNode) ->
            traverseTree ((varName, value) :: currentPath) (id :: currentIds) childNode)

type KnowledgeItem =
    { Number: int
      Conditions: string
      Path: string }

let generateKnowledgeBase (root: Node) =
    traverseTree [] [] root
    |> List.mapi (fun index (num, conditions, path) ->
        { Number = index + 1
          Conditions = conditions
          Path = path })
