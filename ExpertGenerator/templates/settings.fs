module ExpertGenerator.templates.settings

open Microsoft.AspNetCore.Http
open Oxpecker
open XmlModels
open Oxpecker.ViewEngine

let html (ctx: HttpContext) =
    let mxFile = ctx.Items["mxFile"] :?> MxFile
    html(lang = "ru"){
        
    }|> ctx.WriteHtmlView