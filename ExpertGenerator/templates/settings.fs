module ExpertGenerator.templates.settings

open Oxpecker.ViewEngine
open Oxpecker.Htmx
open XmlModels

let html (diagrams: (string * string) list) =
    html (lang = "en") {
        head () {
            title () { "Settings" }
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

        body (hxBoost = true) {
            div (class' = "container") {
                h1 () { "Выберите страницу" }

                form (action = "/generate", method = "post", hxBoost=true) {
                    select (name = "diagram") {
                        for i, d in diagrams |> List.indexed do
                            option (value = string i) { $"{snd d}" }
                    }
                    button (type' = "submit", style = "margin-left: 10px") { "Select" }
                }
            }

        }
    }
