module Client

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open Shared

// The model holds data that you want to keep track of while the application is running
// in this case, we are keeping track of a counter
// we mark it as optional, because initially it will not be available from the client
// the initial value will be requested from server
type Model = { WrittenText: string; CurrentText:string; Added: bool; Lists: string list }

//type ModelText = {WrittenText: WrittenText option}

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
type Msg =
    | AddToList
    | RemoveFromList of string
    | UpdateText of string

let initialCounter () = Fetch.fetchAs<Counter> "/api/init"

let currentText = ""

// defines the initial state and initial command (= side-effect) of the application
let init () : Model * Cmd<Msg> =
    let initialModel = {WrittenText = "";CurrentText = ""; Added = false; Lists = []}
    initialModel, Cmd.none


let filter item removedItem  = item <> removedItem
// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =


    match msg with
    | UpdateText x -> { currentModel with WrittenText = x }, Cmd.none
    | AddToList ->
        let newList = currentModel.WrittenText:: currentModel.Lists
        { currentModel with Added = true; Lists = newList; WrittenText = ""}, Cmd.none
    | RemoveFromList removedItem ->
        let removeList = currentModel.Lists |> List.filter( fun x -> filter x removedItem  )
        { currentModel with Added = false; Lists = removeList}, Cmd.none



(*let update2 (msg : Msg) (modelText : ModelText) : ModelText * Cmd<Msg> =
    match modelText.WrittenText, msg with
    | Some text, AddToList ->
        let newText = {modelText with WriteText = Some {h1 [][str string]}}
        newText, Cmd.none*)

let safeComponents =
    let components =
        span [ ]
           [ a [ Href "https://github.com/SAFE-Stack/SAFE-template" ]
               [ str "SAFE  "
                 str Version.template ]
             str ", "
             a [ Href "https://github.com/giraffe-fsharp/Giraffe" ] [ str "Giraffe" ]
             str ", "
             a [ Href "http://fable.io" ] [ str "Fable" ]
             str ", "
             a [ Href "https://elmish.github.io" ] [ str "Elmish" ]
             str ", "
             a [ Href "https://fulma.github.io/Fulma" ] [ str "Fulma" ]

           ]

    span [ ]
        [ str "Version "
          strong [ ] [ str Version.app ]
          str " powered by: "
          components ]

let box txt =
    Button.button
         [ Button.IsFullWidth
           Button.Color IsPrimary ]
         [ str txt ]

// let button txt onClick =
//     Button.button
//         [ Button.IsFullWidth
//           Button.IsOutlined
//           Button.Color IsPrimary
//           Button.OnClick onClick ]
//         [ str txt ]

let viewSafe (model : Model) (dispatch : Msg -> unit) =
    div [][ ]
    //     Navbar.navbar [ Navbar.Color IsPrimary ]
    //         [ Navbar.Item.div [ ]
    //             [ Heading.h2 [ ]
    //                 [ str "Pryo!" ] ] ]

    //       Container.container []
    //           [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
    //                 [ Heading.h3 [] [ str ("Press buttons to manipulate counter: " + show model) ] ]
    //              Columns.columns []
    //                  [ Column.column [] [ button "divide" (fun _ -> dispatch Decrement) ]
    //                    Column.column [] [ button "multiply" (fun _ -> dispatch Increment) ] ] ]

    //       Footer.footer [ ]
    //             [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
    //                 [ safeComponents ] ]
    // ] ]
(*let mkli n =
    [
        for i in [0..n] do
            li [] [ str (string i)]
    ]*)

let add x = x + 1

let add1 = List.map add

let string = ""

let view (modelText : Model) (dispatch : Msg -> unit)=
    div [] [
        Navbar.navbar [ Navbar.Color IsDark ]
             [ Navbar.Item.div [ ]
                 [ Heading.h2 [ ]
                     [ str "Pryo!"] ]
             ]
        h1 [] [
             Columns.columns []
                [ Column.column [] [ box "TO DO" ]]
        ]
        br []
        ul [] [
            li [] [
                input [ Type "text"; Value modelText.WrittenText; OnChange ( fun ev -> dispatch ( UpdateText ev.Value ) ) ]
                button [OnClick(fun _ -> dispatch AddToList); ] [str "add to list"; ]
            ]
            li [] [
                str modelText.WrittenText
            ]

            for x in modelText.Lists do
                li [] [
                input [Type ""; Value x]
                button [OnClick(fun _ -> dispatch (RemoveFromList x))] [str "X"]
            ]
        ] //@ mkli 9)
    ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
