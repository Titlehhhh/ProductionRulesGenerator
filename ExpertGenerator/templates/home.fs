module ExpertGenerator.templates.home

open Microsoft.AspNetCore.Http
open Oxpecker
open Oxpecker.ViewEngine

let html (ctx: HttpContext) =
    html(lang="ru") {
        head() {
            title() {"Помощник для эксперта"}
            meta(charset="utf-8")
            meta(name="viewport", content="width=device-width, initial-scale=1.0")
            base'(href="/")
            // link(rel="stylesheet", href="/bootstrap/bootstrap.min.css")
            // link(rel="stylesheet", href="/app.css")
            // link(rel="icon", type'="image/png", href="/favicon.png")
            // script(src="https://unpkg.com/htmx.org@1.9.10", crossorigin="anonymous")
            // script(defer=true, src="https://cdn.jsdelivr.net/npm/alpinejs@3.13.5/dist/cdn.min.js", crossorigin="anonymous")
        }
        
        body() {
            "Hi"
//             div(class' = "container"){
//                 h1() { "Upload Your Draw.io Diagram"}
//                 div(id="upload-area", class'="upload-area") {
//                     p() { "Drop your .drawio file here, or click to select" }
//                     form(action="/upload", method="post", enctype="multipart/form-data") {
//                         label(for'="file-upload", class'="file-label") { "Choose File" }
//                         input(type'="file", id="file-upload", name="file")
//                     }
//                 }
//             }
//             script(){
//                 "const uploadArea = document.getElementById('upload-area');
//                 const fileInput = document.getElementById('file-upload');
//
//                 uploadArea.addEventListener('dragover', (e) => {
//                     e.preventDefault();
//                     uploadArea.classList.add('dragover');
//                 });
//
//                 uploadArea.addEventListener('dragleave', () => {
//                     uploadArea.classList.remove('dragover');
//                 });
//
//                 uploadArea.addEventListener('drop', (e) => {
//                     e.preventDefault();
//                     uploadArea.classList.remove('dragover');
//
//                     const files = e.dataTransfer.files;
//                     if (files.length) {
//                         fileInput.files = files;
//                         fileInput.closest('form').submit();
//                     }
//                 });
//
//                 uploadArea.addEventListener('click', () => fileInput.click());"
//             }
        }
    }
