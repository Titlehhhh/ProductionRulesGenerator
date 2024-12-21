open ExpertGenerator.templates
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Oxpecker
open XmlModels


let uploadFile (ctx: HttpContext) =
    let file = ctx.Request.Form.Files["file"]

    ctx.Items["mxFile"] = ParserDrawIO.DeserializeFile(file.OpenReadStream())
    ctx |> redirectTo "/settings" false

let endpoints = [
    GET [
        route "/" <| (htmlView home.html)
        route "/settings" <| settings.html
    ]
    POST [ route "/upload" <| uploadFile ]
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
