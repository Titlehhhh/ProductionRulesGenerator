module ExpertGenerator.templates.main

open System.Net.Http
open Microsoft.AspNetCore.Http
open XmlModels

let html (ctx: HttpContext) =
    let mxFile = ctx.Items["mxFile"] :?> MxFile
    ignore