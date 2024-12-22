module ExpertGenerator.Models


type Comparison =
    { VarName: string
      Value: string }

    override this.ToString() = $"{this.VarName} = \"{this.Value}\""

    member this.Clone() =
        { VarName = this.VarName
          Value = this.Value }

type Condition =
    { Comparisons: Comparison list
      Result: string
      ResultVarName: string }

    override this.ToString() =
        let comparasions =
            this.Comparisons |> List.map _.ToString() |> String.concat " И "

        $"ЕСЛИ {comparasions} ТО {this.ResultVarName} = \"{this.Result}\""

    member this.Clone() =
        { Comparisons =
            this.Comparisons
            |> List.map (fun x ->
                { x with
                    VarName = x.VarName
                    Value = x.Value })
          Result = this.Result
          ResultVarName = this.ResultVarName }

    member this.AllVariables() =
        this.Comparisons |> List.map _.VarName |> String.concat ", "

type Knowledge =
    { Number: int
      Condition: Condition
      Path: string }

    member this.GetStringCondition() = this.Condition.ToString()

    member this.Clone() =
        { Number = this.Number
          Condition = this.Condition.Clone()
          Path = this.Path }
