namespace AlphaProject

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI
open WebSharper.UI.Server
open WebSharper.JavaScript

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/userReg">] UserReg 
    | [<EndPoint "/singIn">] SingIn
    | [<EndPoint "/carReg">] CarReg
    | [<EndPoint "/carStatus">] CarStatus
    

module Templating =
    open WebSharper.UI.Html

    // Compute a menubar where the menu item for the given endpoint is active
    let MenuBar (ctx: Context<EndPoint>) endpoint : Doc list =        
        let ( => ) txt act =
            let isActive = if endpoint = act then "nav-link active" else "nav-link" 
            li [attr.``class`` "nav-item"] [
                a [
                    attr.``class`` isActive
                    attr.href (ctx.Link act)
                ] [text txt]                   
            ]            
            
        [
            "Home" => EndPoint.Home
            "Registration" => EndPoint.UserReg
            "Sign in" => EndPoint.SingIn
            "Register Car" => EndPoint.CarReg
            "Car Status" => EndPoint.CarStatus   
            
        ]

    let Main ctx action (title: string) (body: Doc list) =
        Templates.MainTemplate()
            .Title(title)
            .MenuBar(MenuBar ctx action)
            .Body(body)
            .Doc()

    let UserRegist ctx action (title: string) (body: Doc list) =
        Templates.UserRegTemplate()
            .Title(title)
            .MenuBar(MenuBar ctx action)
            .Body(body)
            .Doc()

    let SigningIn ctx action (title: string) (body: Doc list)=
        Templates.SignInTemplate()
            .Title(title)
            .MenuBar(MenuBar ctx action)
            .Body(body)
            .Doc()    

    let CarRegistration ctx action (title: string) (body: Doc list)=
        Templates.AddCarTemplate()
            .Title(title)
            .MenuBar(MenuBar ctx action)
            .Body(body)
            .Doc() 
    
    let CarStatus ctx action (title: string) (body: Doc list)=
        Templates.CStatusTemplate()
            .Title(title)
            .MenuBar(MenuBar ctx action)
            .Body(body)
            .Doc() 
    

module Site =
    open WebSharper.UI.Html

    open type WebSharper.UI.ClientServer

    let HomePage ctx =
        Content.Page(
            Templating.Main ctx EndPoint.Home "Home" [
                //h1 [] [text "Say Hi to the server!"]
                div [] [client (Client.Main())]
            ], 
            Bundle = "home"
        )

    let UserRegist ctx =
        Content.Page(
            Templating.UserRegist ctx EndPoint.UserReg "UserRegistration" [
                h1 [] [text "UserRegistration"]  
                div [] [client (Client.UserRegistration())]
            ], 
            Bundle = "userReg"
        )

    let SigningInPage ctx =
        Content.Page(
            Templating.SigningIn ctx EndPoint.SingIn "signing In" [
                h1 [] [text "Signing In"]  
                div [] [client (Client.SingingIn())]
            ]
        )

    let CarRegistPage ctx =
        Content.Page(
            Templating.CarRegistration ctx EndPoint.CarReg "register car" [
                h1 [] [text "Register Car"]  
                div [] [client (Client.RegisterCar())]                
            ],
            Bundle = "carReg"
        )

    let CarStatusPage ctx =
        Content.Page(
            Templating.CarStatus ctx EndPoint.CarStatus "car status" [
                h1 [] [text "Check Car Status"]  
                div [] [client (Client.CarStatus())]
            ],
            Bundle = "carStatus"
        )


    [<Website>]
    let Main =
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home -> HomePage ctx
            | EndPoint.UserReg -> UserRegist ctx
            | EndPoint.SingIn -> SigningInPage ctx
            | EndPoint.CarReg -> CarRegistPage ctx
            | EndPoint.CarStatus -> CarStatusPage ctx
        )
