module ExpertGenerator.Database

open System
open System.Collections.Generic
open XmlModels
type File = {
    Id: string
    File: byte[]
    Type: string
    Name: string
}
type User = {
    Id: Guid
    MxFile: MxFile
    Tree: Tree
    Files: Map<string, File>
}

let private dict = Dictionary<Guid, User>()



let add (user: User) =
    dict.[user.Id] <- user

let get (id: Guid) =
    dict.[id]

let remove (id: Guid) =
    dict.Remove(id)

let getAll () =
    dict.Values 
