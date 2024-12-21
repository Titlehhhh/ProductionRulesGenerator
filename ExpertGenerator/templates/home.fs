module ExpertGenerator.templates.home

open Oxpecker.ViewEngine

let html =
    html (lang = "en") {
        head () {
            title () { "Upload Draw.io Diagram" }
            meta (charset = "utf-8")
            meta (name = "viewport", content = "width=device-width, initial-scale=1.0")
            base' (href = "/")
            link (rel = "stylesheet", href = "/bootstrap/bootstrap.min.css")
            link (rel = "stylesheet", href = "/app.css")
            script (src = "https://unpkg.com/htmx.org@1.9.10", crossorigin = "anonymous")            
            script (
                defer = true,
                src = "https://cdn.jsdelivr.net/npm/alpinejs@3.13.5/dist/cdn.min.js",
                crossorigin = "anonymous"
            )              
        }
        body () {
            div (class' = "container") {
                h1 () { "Upload Your Draw.io Diagram" }
                div (id = "upload-area", class' = "upload-area") {
                    p () { "Drag & drop your file here, or click to select" }
                    form (action = "/upload", method = "post", enctype = "multipart/form-data") {
                        label (for' = "file-upload", class' = "file-label") { "Choose File" }
                        input (type' = "file", id = "file-upload", name = "file")
                    }                     
                }
            }
            script (src = "/script.js")    
        }
    }