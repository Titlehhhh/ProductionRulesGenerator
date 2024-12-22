module ExpertGenerator.templates.start

open System.Net.Http
open Microsoft.AspNetCore.Http
open XmlModels

let html (ctx: HttpContext) =
    
    ignore