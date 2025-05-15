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
    type UserPage = Templating.Template<"UserDataPage.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>
    type StatusChange = Templating.Template<"ChangingCarStatus.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>

[<JavaScript>]
module Client =
     let userEmail = Var.Create ""
     let password = Var.Create ""    
     let sessionId = Var.Create ""

     let Main () =  
        Server.EnableParallelWrite()
        
        userEmail := JS.Window.SessionStorage.GetItem("userEmail")
        let userPermission = Var.Create ""

        if userEmail.Value <> "" && userEmail.Value <> null then
            let menuEmail = JS.Document.GetElementById("LoginEmail")   
            menuEmail.SetAttribute("style", "visibility: visible")
            menuEmail.TextContent <- userEmail.Value


            async{
                let! perRes = Server.GetUserPermission userEmail.Value
                userPermission := perRes

                if userPermission.Value = "2" then
                    let statusEmail = JS.Document.GetElementById("StatusChange")
                    statusEmail.SetAttribute("style", "visibility: visible")
        
            }
            |>Async.StartImmediate

        Templates.MainTemplate.MainForm()           
                      
            .Doc()

     let UserRegistration () =        
        let serverRespons = Var.Create ""   
        let userPermission = Var.Create ""
        
        JS.SetTimeout (fun () ->

            userEmail := JS.Window.SessionStorage.GetItem("userEmail")

            if userEmail.Value <> "" && userEmail.Value <> null then
                let menuEmail = JS.Document.GetElementById("LoginEmail")   
                menuEmail.SetAttribute("style", "visibility: visible")
                menuEmail.TextContent <- userEmail.Value

                async{
                    let! perRes = Server.GetUserPermission userEmail.Value
                    userPermission := perRes

                    if userPermission.Value = "2" then
                        let statusEmail = JS.Document.GetElementById("StatusChange")
                        statusEmail.SetAttribute("style", "visibility: visible")
                }
                |> Async.StartImmediate

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
        let userPermission = Var.Create ""
        JS.SetTimeout (fun () ->
            userEmail := JS.Window.SessionStorage.GetItem("userEmail")

            if userEmail.Value <> "" && userEmail.Value <> null then
                let menuEmail = JS.Document.GetElementById("LoginEmail")   
                menuEmail.SetAttribute("style", "visibility: visible")
                menuEmail.TextContent <- userEmail.Value

                async{
                    let! perRes = Server.GetUserPermission userEmail.Value
                    userPermission := perRes

                    if userPermission.Value = "2" then
                        let statusEmail = JS.Document.GetElementById("StatusChange")
                        statusEmail.SetAttribute("style", "visibility: visible")
                }
                |>Async.StartImmediate  

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

                            let menuEmail = JS.Document.GetElementById("LoginEmail")   
                            menuEmail.SetAttribute("style", "visibility: visible")
                            menuEmail.TextContent <- userEmail.Value                  

                        ) 0 |> ignore   
                        

                        let! perRes = Server.GetUserPermission userEmail.Value
                        userPermission := perRes

                        if userPermission.Value = "2" then
                            let statusEmail = JS.Document.GetElementById("StatusChange")
                            statusEmail.SetAttribute("style", "visibility: visible")

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
        let userPermission = Var.Create ""
        let failureData: FailureCosts = ListModel.Create(fun item -> item.failure_name)[] 
        let serverRespons = Var.Create ""
        
        JS.SetTimeout (fun () ->     

            userEmail := JS.Window.SessionStorage.GetItem("userEmail")

            if userEmail.Value <> "" && userEmail.Value <> null then
                let menuEmail = JS.Document.GetElementById("LoginEmail")   
                menuEmail.SetAttribute("style", "visibility: visible")
                menuEmail.TextContent <- userEmail.Value

                async{

                    let! perRes = Server.GetUserPermission userEmail.Value
                    userPermission := perRes

                    if userPermission.Value = "2" then
                        let statusEmail = JS.Document.GetElementById("StatusChange")
                        statusEmail.SetAttribute("style", "visibility: visible")

                }
                |> Async.StartImmediate
            else
                userPermission := "4"

            let faulureSelect = JS.Document.GetElementById("failure") :?> HTMLSelectElement           

            async{
                    let! failureOptions = Server.GetFailureNames()

                    failureData.AppendMany failureOptions

                    let opt = JS.Document.CreateElement("option") :?> HTMLOptionElement
                    opt.Value <- ""
                    opt.Text <- ""
                    faulureSelect.AppendChild(opt) |> ignore 
                    
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

                    if userEmail.Value <> "" then
                        let menuEmail = JS.Document.GetElementById("LoginEmail")   
                        menuEmail.SetAttribute("style", "visibility: visible")
                        menuEmail.TextContent <- userEmail.Value

                    sessionId.Value <- sessionIdResponse

                    let sessionChek = JS.Window.SessionStorage.GetItem("sessionId")  
                     
                    if sessionChek = sessionId.Value then                                           

                        let! userId = Server.CurrentUserId password.Value userEmail.Value

                        let newCar : CarJoinedData =
                            {                        
                            car_licence = StringValidation.removeForbiddenCharacters e.Vars.licence.Value
                            user_id = userId
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
                            isNonEmpty car.user_id &&
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
                 e.Vars.repair_cost.Value <- repairCost
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

            if userEmail.Value <> "" && userEmail.Value <> null then
                let menuEmail = JS.Document.GetElementById("LoginEmail")   
                menuEmail.SetAttribute("style", "visibility: visible")
                menuEmail.TextContent <- userEmail.Value
                async {
                    let! perRes = Server.GetUserPermission userEmail.Value
                    userPermission := perRes

                    if userPermission.Value = "2" then
                        let statusEmail = JS.Document.GetElementById("StatusChange")
                        statusEmail.SetAttribute("style", "visibility: visible")

                }
                |> Async.StartImmediate 

            else
                userPermission := "4"          
                      

        ) 0 |> ignore         
        
        Templates.CStatusTemplate.MainForm()
            .OnSend(fun e ->
                async {     
                    carData.Clear()

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

     let UserDataPage () =
        let serverRespons = Var.Create ""   
        let userPermission = Var.Create ""

        JS.SetTimeout (fun () ->

            userEmail := JS.Window.SessionStorage.GetItem("userEmail")

            if userEmail.Value <> "" && userEmail.Value <> null then
                let menuEmail = JS.Document.GetElementById("LoginEmail")   
                menuEmail.SetAttribute("style", "visibility: visible")
                menuEmail.TextContent <- userEmail.Value

        ) 0 |> ignore        

        async {     
                
                    let! sessionIdResponse = Server.ReturnSessionId()  
                    sessionId.Value <- sessionIdResponse

                    let sessionChek = JS.Window.SessionStorage.GetItem("sessionId")  

                    if sessionChek = sessionId.Value then
                        let! currentUser = Server.CurrentUserData userEmail.Value                                              

                        JS.SetTimeout (fun () ->
                            let inputs = JS.Document.QuerySelectorAll("input")                                

                            (inputs.Item 0 :?> HTMLInputElement).Value <- currentUser.first_name
                            (inputs.Item 1 :?> HTMLInputElement).Value <- currentUser.family_name
                            (inputs.Item 2 :?> HTMLInputElement).Value <- currentUser.phone_number
                            (inputs.Item 3 :?> HTMLInputElement).Value <- currentUser.city
                            (inputs.Item 4 :?> HTMLInputElement).Value <- currentUser.street
                            (inputs.Item 5 :?> HTMLInputElement).Value <- currentUser.house_number
                            (inputs.Item 6 :?> HTMLInputElement).Value <- currentUser.floor_door 

                            
                         ) 0 |> ignore

                    let! perRes = Server.GetUserPermission userEmail.Value
                    userPermission := perRes

                    if userPermission.Value = "2" then
                        let statusEmail = JS.Document.GetElementById("StatusChange")
                        statusEmail.SetAttribute("style", "visibility: visible")
                }
                |> Async.StartImmediate

        Templates.UserPage.MainForm()
            .OnClick(fun e ->                
                let allIputs = JS.Document.QuerySelectorAll("input")
                for i in 0 .. allIputs.Length - 1 do
                    let input = allIputs.[i] :?> HTMLInputElement
                    input.RemoveAttribute("disabled")       
                    
                e.Vars.FirstName.Value <- (allIputs.Item 0 :?> HTMLInputElement).Value
                e.Vars.LastName.Value <- (allIputs.Item 1 :?> HTMLInputElement).Value
                e.Vars.Phone.Value <- (allIputs.Item 2 :?> HTMLInputElement).Value
                e.Vars.City.Value <- (allIputs.Item 3 :?> HTMLInputElement).Value
                e.Vars.Street.Value <- (allIputs.Item 4 :?> HTMLInputElement).Value
                e.Vars.HouseNumber.Value <- (allIputs.Item 5 :?> HTMLInputElement).Value
                e.Vars.FloorDoor.Value <- (allIputs.Item 6 :?> HTMLInputElement).Value
            
            )
            .OnSubmit(fun e ->
                async{                

                    let newUser =          
                        {

                        first_name = StringValidation.removeForbiddenCharacters e.Vars.FirstName.Value
                        family_name = StringValidation.removeForbiddenCharacters e.Vars.LastName.Value
                        password = "notchanged"
                        email = userEmail.Value
                        phone_number = StringValidation.removeForbiddenCharacters e.Vars.Phone.Value
                        main_id = "notchanged"
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
                        isNonEmpty user.phone_number &&
                        isNonEmpty user.email &&
                        isNonEmpty user.city &&
                        isNonEmpty user.street &&
                        isNonEmpty user.house_number &&
                        isNonEmpty user.floor_door
                    
                    let verifiValue = isUserDataComplete newUser              

                    if verifiValue then
                        
                        let! res = Server.UpdateUser newUser
                        serverRespons := res.ToString()                       
                          
                    else
                       
                        let nullResponse = "Please fill in all fields."
                        serverRespons.Value <- nullResponse 
                        
                    JS.Alert(serverRespons.Value)      
                    JS.Window.Location.Reload()
                    
                }
                |> Async.StartImmediate
            
            )
            .Doc()

     let ChangeStatus () =
        let carData: CarsJoinedData = ListModel.Create (fun item -> item.car_licence) [] 
        let failureData: FailureCosts = ListModel.Create(fun item -> item.failure_name)[]         
        let statusData: RepairStatuses = ListModel.Create(fun item -> item.status_name)[]
        let searchEmail = Var.Create ""

        JS.SetTimeout (fun () ->                                  
            let statusSelect = JS.Document.GetElementById("status") :?> HTMLSelectElement
            let faulureSelect = JS.Document.GetElementById("failure") :?> HTMLSelectElement

            userEmail := JS.Window.SessionStorage.GetItem("userEmail")

            if userEmail.Value <> "" && userEmail.Value <> null then
                let menuEmail = JS.Document.GetElementById("LoginEmail")   
                menuEmail.SetAttribute("style", "visibility: visible")
                menuEmail.TextContent <- userEmail.Value

            async{
                let! statusOptions = Server.GetStatusNames()

                statusData.AppendMany statusOptions

                let opt = JS.Document.CreateElement("option") :?> HTMLOptionElement
                opt.Value <- ""
                opt.Text <- ""
                statusSelect.AppendChild(opt) |> ignore
                    
                for value in statusData do
                    let opt = JS.Document.CreateElement("option") :?> HTMLOptionElement
                    opt.Value <- value.status_name
                    opt.Text <- value.status_name
                    statusSelect.AppendChild(opt) |> ignore                    

                let! failureOptions = Server.GetFailureNames()

                failureData.AppendMany failureOptions
                    
                let opt = JS.Document.CreateElement("option") :?> HTMLOptionElement
                opt.Value <- ""
                opt.Text <- ""
                faulureSelect.AppendChild(opt) |> ignore

                for value in failureData do
                    let opt = JS.Document.CreateElement("option") :?> HTMLOptionElement
                    opt.Value <- value.failure_name.ToString()
                    opt.Text <- value.failure_name.ToString()
                    faulureSelect.AppendChild(opt) |> ignore             
            }
            |>Async.StartImmediate
        ) 0 |> ignore 
        
        Templates.StatusChange.MainForm()
            .SearchDatabase(fun e ->
                async{
                    let allSelects = JS.Document.QuerySelectorAll("select")
                    for i = 0 to allSelects.Length - 1 do
                        let sel = allSelects.[i] :?> HTMLSelectElement
                        sel.RemoveAttribute("disabled") 

                    let submitButton = JS.Document.GetElementById("submitButton")
                    submitButton.RemoveAttribute("disabled") 

                    let updateButton = JS.Document.GetElementById("updateButton")
                    updateButton.RemoveAttribute("disabled") 

                    let! sessionIdResponse = Server.ReturnSessionId()  
                    sessionId.Value <- sessionIdResponse

                    let sessionChek = JS.Window.SessionStorage.GetItem("sessionId")  
                     
                    if sessionChek = sessionId.Value then 
                        let! res = Server.GetCarByid e.Vars.searchLicence.Value   
                        let! emailRes = Server.GetUserEmail e.Vars.searchLicence.Value
                        searchEmail:= emailRes                         

                        carData.AppendMany res   
                                                                                                              
                }
                |>Async.StartImmediate
            
            )
            .ListContainer(
                carData.View.DocSeqCached(fun (item: CarJoinedData) ->
                    Templates.StatusChange.ListItem()
                        .licence(item.car_licence)                        
                        .manuf(item.manuf)
                        .c_type(item.c_type)
                        .m_year(item.m_year.ToString())
                        .failure(item.failure) 
                        .repair_cost(item.repair_costs.ToString())
                        .repair_status(item.repair_status)
                        .user_email(searchEmail.Value)
                        .Doc()                
                )
            )      
            .OnChange(fun e ->               
                 
                 let failureName = e.Vars.failure.Value               
                 
                 let failureCost =
                    failureData |> Seq.tryFind (fun item -> item.failure_name = failureName)

                 let repairCost =
                    match failureCost with
                    | Some failure -> failure.repair_costs
                    | None -> 0.0
                 e.Vars.repair_cost.Value <- repairCost
                 JS.SetTimeout (fun () ->
                    
                    let costInput = JS.Document.GetElementById("repair_cost") :?> HTMLInputElement
                    costInput.Value <- repairCost.ToString()
                    
                 ) 0 |> ignore            

            )
            .OnSubmit(fun e ->
                async{
                    let currentStatus = e.Vars.status.Value
                    let licence = e.Vars.searchLicence.Value

                    let! res = Server.UpdateCarStatus licence currentStatus

                    if currentStatus = "Handover" then
                        let archiveCar:CarJoinedData = carData.Value |> Seq.head                       


                        let! archiveResponse = Server.InsertIntoArchive(archiveCar, searchEmail.Value)
                        let! deleteFromCars = Server.DeleteCarFromDatabase licence                     

                        JS.Alert(archiveResponse)
                        JS.Alert(deleteFromCars)

                    JS.Alert(res.ToString()) 
                    JS.Window.Location.Reload()              

                }
                |>Async.StartImmediate
            
            )   
            .OnUpdate(fun e ->
                async{                  

                    let currentMalf = e.Vars.failure.Value
                    let licence = e.Vars.searchLicence.Value
                    let currentCost = e.Vars.repair_cost.Value

                    let! failureRes = Server.UpdateCarMalfunction licence currentMalf

                    JS.Alert(failureRes.ToString()) 

                    let! costRes = Server.UpdateRepairCost licence currentCost

                    JS.Alert(costRes.ToString())  

                }
                |>Async.StartImmediate                        
            )
            .OnSave(fun e ->
                async{                   
                    
                    let newFailureName = e.Vars.failureName.Value
                    let newDesc = e.Vars.failureDesc.Value
                    let newCost = e.Vars.failureCost.Value
                    if newFailureName <> "" && newDesc <> "" && newCost > 0.0 then
                        let! insertRes = Server.InserNewMalfunction (newFailureName, newDesc, newCost)

                        JS.Alert(insertRes.ToString()) 
                        JS.Window.Location.Reload()  
                    else
                        JS.Alert("Please Fill in all fields!")
                }
                |>Async.StartImmediate                        
            )
            .SearchArchive(fun e ->
                async{ 
                    let! res = Server.GetCarFromArchive e.Vars.searchLicence.Value   

                    carData.AppendMany res   
                    searchEmail:= res.Head.user_id
                    
                    let allSelects = JS.Document.QuerySelectorAll("select")
                    for i = 0 to allSelects.Length - 1 do
                        let sel = allSelects.[i] :?> HTMLSelectElement
                        sel.SetAttribute("disabled", "")    

                    let submitButton = JS.Document.GetElementById("submitButton")
                    submitButton.SetAttribute("disabled", "") 

                    let updateButton = JS.Document.GetElementById("updateButton")
                    updateButton.SetAttribute("disabled", "")

                }
                |>Async.StartImmediate
            )            
            .Doc()