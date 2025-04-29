namespace AlphaProject
open System
open System.IO
open WebSharper
open WebSharper.UI
open WebSharper.UI.Templating
open WebSharper.UI.Notation
open WebSharper.JavaScript
open WebSharper.UI.Client

[<JavaScript>]
module Templates =

    type MainTemplate = Templating.Template<"Main.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>
    type UserRegTemplate = Templating.Template<"UserRegist.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>
    type SignInTemplate = Templating.Template<"LoginPage.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>
    type AddCarTemplate = Templating.Template<"CarRegist.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>
    type CStatusTemplate = Templating.Template<"CarStatus.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>

[<JavaScript>]
module Client =
     let userEmail = Var.Create ""
     let password = Var.Create ""    
     let sessionId = Var.Create ""

     let Main () =               

        Templates.MainTemplate.MainForm()           
                      
           .Doc()

     let UserRegistration () =        
        let serverRespons = Var.Create ""        
        
        JS.SetTimeout (fun () ->
            userEmail := JS.Window.SessionStorage.GetItem("userEmail")
            let menuEmial = JS.Document.GetElementById("LoginEmail")
            menuEmial.TextContent <- userEmail.Value
        ) 0 |> ignore   


        Templates.UserRegTemplate.MainForm()            
            .OnSubmit(fun e ->                
                async{                  
 
                    let newUser =          
                        {
                        first_name = StringValidation.removeForbiddenCharacters e.Vars.FirstName.Value
                        family_name = StringValidation.removeForbiddenCharacters e.Vars.LastName.Value
                        password = StringValidation.removeForbiddenCharacters e.Vars.Password.Value
                        email = StringValidation.removeForbiddenCharacters e.Vars.Email.Value
                        phone_number = StringValidation.removeForbiddenCharacters e.Vars.Phone.Value
                        main_id = ""
                        city = StringValidation.removeForbiddenCharacters e.Vars.City.Value
                        street = StringValidation.removeForbiddenCharacters e.Vars.Street.Value
                        house_number = StringValidation.removeForbiddenCharacters e.Vars.HouseNumber.Value
                        floor_door = StringValidation.removeForbiddenCharacters e.Vars.FloorDoor.Value
                        permission = 4   
                        
                    }                   
                    
                    let isUserDataComplete (user: UserData) =
                        let isNonEmpty (s: string) = not (String.IsNullOrWhiteSpace s)    
                        
                        isNonEmpty user.family_name &&
                        isNonEmpty user.first_name &&
                        isNonEmpty user.password &&
                        isNonEmpty user.phone_number &&
                        isNonEmpty user.email &&
                        isNonEmpty user.city &&
                        isNonEmpty user.street &&
                        isNonEmpty user.house_number &&
                        isNonEmpty user.floor_door
                    
                    let verifiValue = isUserDataComplete newUser

                    if verifiValue then

                        let! res = Server.RegisterNewUser newUser
                        serverRespons := res                       
                            
                    else
                        let nullRespnse = "Please fill in all fields."
                        serverRespons.Value <- nullRespnse 
                        
                    JS.Alert(serverRespons.Value)      
                    JS.Window.Location.Reload()
                    
                }
                |> Async.StartImmediate
                
            )
            .Doc()

     let SingingIn () =      

        JS.SetTimeout (fun () ->
            userEmail := JS.Window.SessionStorage.GetItem("userEmail")
            let menuEmial = JS.Document.GetElementById("LoginEmail")
            menuEmial.TextContent <- userEmail.Value
        ) 0 |> ignore          

        Templates.SignInTemplate.MainForm()
            .OnSubmit(fun e ->
                async{
                    JS.Window.SessionStorage.RemoveItem("userEmail")
                    password := StringValidation.removeForbiddenCharacters e.Vars.Password.Value
                    userEmail := StringValidation.removeForbiddenCharacters e.Vars.Email.Value 

                    let! result = Server.LogingInToDatabase password.Value userEmail.Value                    

                    match result with
                    | Some sid ->
                        sessionId := sid                        
                        JS.Window.SessionStorage.SetItem("sessionId", sid)                            

                        JS.Window.SessionStorage.SetItem("userEmail", userEmail.Value)                                             

                        JS.SetTimeout (fun () ->
                            userEmail := JS.Window.SessionStorage.GetItem("userEmail")
                            let menuEmial = JS.Document.GetElementById("LoginEmail")
                            menuEmial.TextContent <- userEmail.Value
                        ) 0 |> ignore                        

                        let welcomeMsg = $"Welcome: {userEmail.Value}"
                        JS.Alert(welcomeMsg)
                    | None ->
                        JS.Alert("No user found")                                 

                    e.Vars.Password.Value <-""
                    e.Vars.Email.Value <-""              
                                        
                }
                |> Async.StartImmediate

            )    
            .Doc()

     let RegisterCar () =     
        
        let failureData: FailureCosts = ListModel.Create(fun item -> item.failure_name)[] 
        let serverRespons = Var.Create ""
        
        JS.SetTimeout (fun () ->     

            userEmail := JS.Window.SessionStorage.GetItem("userEmail")
            let menuEmial = JS.Document.GetElementById("LoginEmail")
            menuEmial.TextContent <- userEmail.Value

            let faulureSelect = JS.Document.GetElementById("failure") :?> HTMLSelectElement           

            async{
                    let! failureOptions = Server.GetFailureNames()

                    failureData.AppendMany failureOptions
                    
                    for value in failureData do
                        let opt = JS.Document.CreateElement("option") :?> HTMLOptionElement
                        opt.Value <- value.failure_name.ToString()
                        opt.Text <- value.failure_name.ToString()
                        faulureSelect.AppendChild(opt) |> ignore
                    }
                    |> Async.StartImmediate
            ) 0 |> ignore         

        Templates.AddCarTemplate.MainForm()            
            .OnSubmit(fun e ->
                async{     
                    let! sessionIdResponse = Server.ReturnSessionId()                          
                    
                    userEmail := JS.Window.SessionStorage.GetItem("userEmail")
                    let menuEmial = JS.Document.GetElementById("LoginEmail")
                    menuEmial.TextContent <- userEmail.Value 

                    sessionId.Value <- sessionIdResponse

                    let sessionChek = JS.Window.SessionStorage.GetItem("sessionId")  
                     
                    if sessionChek = sessionId.Value then                                           

                        let! userId = Server.CurrentUserId password.Value userEmail.Value

                        let newCar : CarJoinedData =
                            {                        
                            car_licence = StringValidation.removeForbiddenCharacters e.Vars.licence.Value
                            user_id = int64 userId
                            manuf = StringValidation.removeForbiddenCharacters e.Vars.manuf.Value
                            c_type = StringValidation.removeForbiddenCharacters e.Vars.c_type.Value
                            m_year = int64 e.Vars.m_year.Value                         
                            failure = e.Vars.failure.Value
                            repair_costs = e.Vars.repair_cost.Value
                            repair_status = "1"
                        }

                        let isCarDataComplete (car: CarJoinedData) =
                            let isNonEmpty (s: string) = not (String.IsNullOrWhiteSpace s)    
                        
                            isNonEmpty car.car_licence &&
                            car.user_id > 0L &&
                            isNonEmpty car.c_type &&
                            car.m_year > 1900L &&     
                            isNonEmpty car.manuf &&
                            isNonEmpty car.failure &&
                            car.repair_costs >= 0.0 &&
                            isNonEmpty car.repair_status                        
                    
                        let verifiValue = isCarDataComplete newCar

                        if verifiValue then
                            let! res = Server.InsertCarData newCar
                            serverRespons := res                            

                        else
                            let nullRespnse = "Please fill in all fields."
                            serverRespons.Value <- nullRespnse 

                        JS.Alert(serverRespons.Value)   
                        JS.Window.Location.Reload()

                }
                |> Async.StartImmediate

            ) 
           .OnChange(fun e ->               
                 
                 let failureName = e.Vars.failure.Value               
                 
                 let failureCost =
                    failureData |> Seq.tryFind (fun item -> item.failure_name = failureName)

                 let repairCost =
                    match failureCost with
                    | Some failure -> failure.repair_costs
                    | None -> 0.0

                 JS.SetTimeout (fun () ->
                    
                    let costInput = JS.Document.GetElementById("repair_cost") :?> HTMLInputElement
                    costInput.Value <- repairCost.ToString()
                    
                 ) 0 |> ignore              
                    
             )            
            .Doc()

     let CarStatus () =    
        let carData: CarsJoinedData = ListModel.Create (fun item -> item.car_licence) [] 
        let statusData : RepairStatuses = ListModel.Create (fun item -> item.status_name) []
        let serverRespons = Var.Create ""      
        let userPermission = Var.Create ""
        
        JS.SetTimeout (fun () ->            
            userEmail := JS.Window.SessionStorage.GetItem("userEmail")
            let menuEmial = JS.Document.GetElementById("LoginEmail")
            menuEmial.TextContent <- userEmail.Value

            if userEmail.Value = "" || userEmail.Value = null then
                userPermission := "4"
            else
                async {
                    let! perRes = Server.GetUserPermission userEmail.Value
                    userPermission := perRes

                }
                |> Async.StartImmediate       
           
        ) 0 |> ignore         
        
        Templates.CStatusTemplate.MainForm()
            .OnSend(fun e ->
                async {     
                
                    let! sessionIdResponse = Server.ReturnSessionId()  
                    sessionId.Value <- sessionIdResponse

                    let sessionChek = JS.Window.SessionStorage.GetItem("sessionId")  
                     
                    if sessionChek = sessionId.Value then 
                    
                        userEmail := JS.Window.SessionStorage.GetItem("userEmail")
                        let! res = Server.GetCarData userEmail.Value userPermission.Value
                        serverRespons := res.ToString()
                        carData.AppendMany res

                        let! statusOptions = Server.GetStatusNames()                    

                        statusData.AppendMany statusOptions

                        JS.SetTimeout (fun () ->
                            let selects = JS.Document.QuerySelectorAll(".repair_dropdown")                       

                            for i = 0 to selects.Length - 1 do
                                let selectEl = selects.[i] :?> HTMLSelectElement   
                            
                                if userPermission.Value <> "2" then
                                    selectEl.Disabled <- true
                                else
                                    selectEl.Disabled <- false 
                                                       
                                for value in statusData do  
                                    let opt = JS.Document.CreateElement("option") :?> HTMLOptionElement
                                    opt.Value <- value.status_name
                                    opt.Text <- value.status_name                                  
                                    selectEl.Append(opt) |>ignore                             

                            ) 0 |> ignore 
                    
                }
                |> Async.StartImmediate
                
            )        
            .ListContainer(
                carData.View.DocSeqCached(fun (item: CarJoinedData) ->
                    Templates.CStatusTemplate.ListItem()
                        .license(item.car_licence)                        
                        .manuf(item.manuf)
                        .c_type(item.c_type)
                        .m_year(item.m_year.ToString())
                        .failure(item.failure) 
                        .repair_cost(item.repair_costs.ToString())
                        .repair_status(item.repair_status)
                        .Doc()                
                )
            )               
           .Doc()

    