open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Oxpecker
open Oxpecker.ViewEngine
open Oxpecker.Htmx

#nowarn "3391"



let startPage (ctx: HttpContext) =
    html (lang = "en") {
        head () {
            title () { "Upload Draw.io Diagram" }
            meta(charset = "utf-8")
            meta(name = "viewport", content = "width=device-width, initial-scale=1.0")
            base'(href = "/")
            link(rel = "stylesheet", href = "/bootstrap/bootstrap.min.css")
            link(rel = "stylesheet", href = "/app.css")
            script(src = "https://unpkg.com/htmx.org@1.9.10", crossorigin = "anonymous")
            script(defer = true, src = "https://cdn.jsdelivr.net/npm/alpinejs@3.13.5/dist/cdn.min.js", crossorigin = "anonymous")
        }
        
        body() {
            div(class'="container") {
                h1() { "Upload Your Draw.io Diagram" }
                div(id="upload-area", class'="upload-area") {
                    p() { "Drag & drop your file here, or click to select" }
                    form(action="/upload", method="post", enctype="multipart/form-data") {
                        label(for'="file-upload", class'="file-label") { "Choose File" }
                        input(type'="file", id="file-upload", name="file")
                    }
                }
                script() {
                    """
                    const uploadArea = document.getElementById('upload-area');
                    const fileInput = document.getElementById('file-upload');

                    uploadArea.addEventListener('dragover', (e) => {
                        e.preventDefault();
                        uploadArea.classList.add('dragover');
                    });

                    uploadArea.addEventListener('dragleave', () => {
                        uploadArea.classList.remove('dragover');
                    });

                    uploadArea.addEventListener('drop', (e) => {
                        e.preventDefault();
                        uploadArea.classList.remove('dragover');

                        const files = e.dataTransfer.files;
                        if (files.length) {
                            fileInput.files = files;
                            fileInput.closest('form').submit();
                        }
                    });

                    uploadArea.addEventListener('click', () => fileInput.click());
                    """
                }
            }
        }

    }

let htmlView' (ctx: HttpContext) = ctx.WriteHtmlView(startPage (ctx))

let endpoints = [ GET [ route "/" <| htmlView' ] ]


let configureApp (appBuilder: WebApplication) =
    if appBuilder.Environment.IsDevelopment() then
        appBuilder.UseDeveloperExceptionPage() |> ignore
    else
        appBuilder.UseExceptionHandler("/error", true) |> ignore
    appBuilder
        .UseStaticFiles()
        .UseAntiforgery()
        .UseRouting()
        .UseOxpecker(endpoints) |> ignore

let configureServices (services: IServiceCollection) =
    services
        .AddRouting()
        .AddLogging(fun builder -> builder.AddFilter("Microsoft.AspNetCore", LogLevel.Warning) |> ignore)
        .AddAntiforgery()
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
