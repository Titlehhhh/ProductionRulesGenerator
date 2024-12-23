module ExpertGenerator.templates.resultTables

open ExpertGenerator.TableGenerator
open Oxpecker.ViewEngine
open Oxpecker.Htmx


let html (variables: ResultItem array) (knownVariables: KnowledgeItem seq) =

    html (lang = "en") {
        head () {
            title () { "Generate" }
            meta (charset = "utf-8")
            meta (name = "viewport", content = "width=device-width, initial-scale=1.0")
            base' (href = "/")
            link (rel = "stylesheet", href = "/bootstrap/bootstrap.min.css")
            link (rel = "stylesheet", href = "/result.css")
            script (src = "https://unpkg.com/htmx.org@1.9.12", crossorigin = "anonymous")
            script (src = "/diagram.js")            
            script (src = "/test.js")

            script (
                defer = true,
                src = "https://cdn.jsdelivr.net/npm/alpinejs@3.13.5/dist/cdn.min.js",
                crossorigin = "anonymous"
            )


        }

        body().on("load","loadDiagram()") {
            div (class' = "demoTabs") {
                input (class' = "demoTabs__tab", type' = "radio", name = "demoTab", id = "demoTab-1", checked' = true)
                label (class' = "demoTabs__label", for' = "demoTab-1") { "Таблицы" }
                input (class' = "demoTabs__tab", type' = "radio", name = "demoTab", id = "demoTab-2")
                label (class' = "demoTabs__label", for' = "demoTab-2") { "Диаграмма" }

                div (class' = "demoTabs__content") {
                    // Таблицы
                    div(class' = "demoTabs__area grid-container").attr ("data-tab", "demoTab-1") {
                        div (class' = "table-wrapper") {
                            div (class' = "table-header") {
                                h2 () { "Переменные" }

                                button(class' = "download-button").on ("click", "downloadFile(\"variables\")") {
                                    "Загрузить"
                                }
                            }

                            div (class' = "table-content") {
                                table () {
                                    thead () {
                                        tr () {
                                            th () { "Номер" }
                                            th () { "Имя" }
                                            th () { "Значение" }
                                        }
                                    }

                                    tbody () {
                                        for variable in variables do
                                            tr () {
                                                td () { variable.Id }
                                                td () { variable.Name }
                                                td () { variable.Value }
                                            }
                                    }
                                }
                            }
                        }

                        div (class' = "table-wrapper", style = "margin-top: 20px;") {
                            div (class' = "table-header") {
                                h2 () { "База знаний" }

                                button(class' = "download-button").on ("click", "downloadFile(\"knowledges\")") {
                                    "Загрузить"
                                }
                            }

                            div (class' = "table-content") {
                                table () {
                                    thead () {
                                        tr () {
                                            th () { "Номер" }
                                            th () { "Знание" }
                                            th () { "Путь" }
                                        }
                                    }

                                    tbody () {
                                        for k in knownVariables do
                                            tr () {
                                                td () { string k.Number }
                                                td () { k.Conditions }
                                                td () { k.Path }
                                            }
                                    }
                                }
                            }
                        }
                    }


                    // Диаграмма
                    div(class' = "demoTabs__area").attr ("data-tab", "demoTab-2") {
                        div(id = "diagram-area") {
                            //iframe(id="diagram-iframe", src="https://embed.diagrams.net/?embed=1", class'="diagram")
                        }
                        
                    }
                }
            }

        }


    }
