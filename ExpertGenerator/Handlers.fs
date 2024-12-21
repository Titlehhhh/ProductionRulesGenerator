module ExpertGenerator.Handlers

open System
open System.Collections.Generic
open System.Text
open System.Threading.Tasks
open ExpertGenerator.templates
open Microsoft.AspNetCore.Http
open Oxpecker

let settings (ctx: HttpContext) =
    match ctx.Request.Cookies.TryGetValue("UserID") with
    | true, userID ->
        let user = Database.get (Guid.Parse(userID))
        let diagrams = user.MxFile.Diagram                       
                       |> Seq.map (fun diagram -> (diagram.Id, diagram.Name))
                       |> Seq.toList
        let html = settings.html diagrams
        ctx.WriteHtmlView html
    | _ -> Task.CompletedTask
