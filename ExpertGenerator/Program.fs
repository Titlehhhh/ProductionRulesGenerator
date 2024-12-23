open System
open System.IO
open System.Text
open System.Threading.Tasks
open ExpertGenerator
open ExpertGenerator.Database
open ExpertGenerator.templates
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Oxpecker
open Oxpecker.ViewEngine
open XmlModels
open Database


let uploadFile (ctx: HttpContext) =
    let file = ctx.Request.Form.Files["file"]
    use stream = file.OpenReadStream()    
    let mxFile = ParserDrawIO.DeserializeFile(stream)    
    let user: User = {
        Id = Guid.NewGuid()
        MxFile = mxFile
        Tree = null
        Files = Map.empty
        SelectedIndex = 0
    }
    Database.add user
    ctx.Response.Cookies.Append("UserID", user.Id.ToString())
    ctx |> redirectTo "/settings" false

let generate (ctx: HttpContext) =
    let selectedIndex = ctx.Request.Form.["diagram"].ToString()
    let user = Database.get (Guid.Parse(ctx.Request.Cookies["UserID"]))
    let diagram = user.MxFile.Diagram.[int selectedIndex]
    let tree = ParserDrawIO.Parse(diagram, user.MxFile)
    Database.add { user with Tree = tree; SelectedIndex = selectedIndex |> int }
    ctx.Response.Headers.Add("HX-Redirect", "/result")
    Task.CompletedTask

let result (ctx: HttpContext) =
    let userId = Guid.Parse(ctx.Request.Cookies["UserID"])
    let user = Database.get (userId)
    let tree = user.Tree
    let variables = TableGenerator.getVariables tree
    let knownVariables = TableGenerator.generateKnowledgeBase tree.Root
    
    let variablesFile = variables
                        |> Seq.map(fun x-> $"{x.Id};{x.Name};{x.Value}")
                        |> String.concat Environment.NewLine
                        |> Encoding.UTF8.GetBytes
    
    let knownVariablesFile = knownVariables
                             |> Seq.map(fun x-> $"{x.Number};{x.Conditions};{x.Path}")
                             |> String.concat Environment.NewLine
                             |> Encoding.UTF8.GetBytes
    
    let markedDiagram = ParserDrawIO.SerializeMxGraphModel(user.MxFile.Diagram[user.SelectedIndex].MxGraphModel)
                        |> Encoding.UTF8.GetBytes
                        
                        
    let filesMap = Map.empty
                    .Add("variables", { Id = "variables"; File = variablesFile; Type = "text/csv"; Name = "variables.csv" })
                    .Add("knowledges", { Id = "knowledges"; File = knownVariablesFile; Type = "text/csv"; Name = "knowledges.csv" })
                    .Add("diagram", { Id = "diagram"; File = markedDiagram; Type = "text/xml"; Name = "diagram.drawio" })
    let newUser = {
        Id = userId
        MxFile = user.MxFile
        SelectedIndex = user.SelectedIndex
        Tree = tree
        Files = filesMap
    }
    
    Database.add newUser   
    
    let html = resultTables.html variables knownVariables
    ctx.WriteHtmlView html
  
let download (id: string) : EndpointHandler =
    fun (ctx: HttpContext) ->
        task {
            let user = Database.get (Guid.Parse(ctx.Request.Cookies["UserID"]))
            let file = user.Files.[id]
            ctx.Response.ContentType <- file.Type
            ctx.Response.Body.WriteAsync file.File |> ignore
        } :> Task
        
    
let endpoints = [
    GET [
        route "/" <| (htmlView home.html)
        route "/settings" <| Handlers.settings
        route "/result" <| result
        routef "/download/{%s}" <| download 
    ]
    POST [ route "/upload" <| uploadFile
           route "/generate" <| generate ]
]

let configureApp (appBuilder: WebApplication) =
    appBuilder.Urls.Add("http://0.0.0.0:5000")

    if appBuilder.Environment.IsDevelopment() then
        appBuilder.UseDeveloperExceptionPage() |> ignore
    else
        appBuilder.UseExceptionHandler("/error", true) |> ignore

    appBuilder.UseStaticFiles().UseAntiforgery().UseRouting().UseOxpecker(endpoints)
    |> ignore


let configureServices (services: IServiceCollection) =
    services
        .AddRouting()
        .AddLogging(fun builder -> builder.AddFilter("Microsoft.AspNetCore", LogLevel.Warning) |> ignore)
        .AddAntiforgery()
        .AddCors()
        .AddOxpecker()
    |> ignore


[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    configureServices builder.Services
    let app = builder.Build()
    configureApp app
    app.Run()
    0
