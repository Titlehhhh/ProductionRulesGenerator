open System
open System.IO
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
    }
    Database.add user
    ctx.Response.Cookies.Append("UserID", user.Id.ToString())
    ctx |> redirectTo "/settings" false

let generate (ctx: HttpContext) =
    let selectedIndex = ctx.Request.Form.["diagram"].ToString()
    let user = Database.get (Guid.Parse(ctx.Request.Cookies["UserID"]))
    let diagram = user.MxFile.Diagram.[int selectedIndex]
    let tree = ParserDrawIO.Parse(diagram, user.MxFile)
    Database.add { user with Tree = tree }
    ctx.Response.Headers.Add("HX-Redirect", "/result")
    Task.CompletedTask

let result (ctx: HttpContext) =
    let user = Database.get (Guid.Parse(ctx.Request.Cookies["UserID"]))
    let tree = user.Tree
    let html = resultTables.html tree
    ctx.WriteHtmlView html
        
let endpoints = [
    GET [
        route "/" <| (htmlView home.html)
        route "/settings" <| Handlers.settings
        route "/result" <| result
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
