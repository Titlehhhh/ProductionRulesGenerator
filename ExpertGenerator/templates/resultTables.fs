module ExpertGenerator.templates.resultTables

open ExpertGenerator
open Oxpecker.ViewEngine

open XmlModels

let html (tree: Tree) =
    let variables = TableGenerator.getVariables tree
    let knownVariables = TableGenerator.generateKnowledgeBase tree.Root
    html (lang = "en") {
        head () {
            title () { "Generate" }
            meta (charset = "utf-8")
            meta (name = "viewport", content = "width=device-width, initial-scale=1.0")
            base' (href = "/")
            link (rel = "stylesheet", href = "/bootstrap/bootstrap.min.css")
            link (rel = "stylesheet", href = "/result.css")
            script (src = "https://unpkg.com/htmx.org@1.9.12", crossorigin = "anonymous")            
            script (
                defer = true,
                src = "https://cdn.jsdelivr.net/npm/alpinejs@3.13.5/dist/cdn.min.js",
                crossorigin = "anonymous"
            )
            script(src = "/test.js") {}
            
        }

        body () {
            div(class'="grid-container") {
                div(class' = "table-wrapper") {
                    div(class'= "table-header") {
                        h2() {"Переменные"}
                        button(class'="download-button") { "Загрузить" }
                    }
                    div(class' = "table-content") {
                        table() {                            
                            thead() {                                
                                tr() {                                    
                                    th() { "Номер" }
                                    th() { "Имя" }
                                    th() { "Значение" }
                                }
                            }
                            tbody() {
                                for variable in variables do
                                    tr() {
                                        td() { variable.Id }
                                        td() { variable.Name }
                                        td() { variable.Value }
                                    }
                            }
                        }
                    }
                }
                

                div(class'= "table-wrapper", style="margin-top: 20px;") {
                    div(class'= "table-header") {
                        h2() {"База знаний"}
                        button(class'="download-button") { "Загрузить" }
                    }
                    div(class' = "table-content") {
                        table() {
                            thead() {
                                tr() {
                                    th() { "Номер" }
                                    th() { "Знание" }
                                    th() { "Путь" }
                                }
                            }
                            tbody() {
                                for k in knownVariables do
                                    tr() {
                                        td() { string k.Number }
                                        td() { k.Conditions }
                                        td() { k.Path }
                                    }
                            }
                        }
                    }
                }
            }
            
        }
    }