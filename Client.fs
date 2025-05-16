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
     let userPermission = Var.Create ""

     let VerifyUser targetUserEmail =
                  
         // If there is a logged-in user, their email is stored in session storage,
         // and a menu item with the stored email will appear in the menu.
         let menuEmail = JS.Document.GetElementById("LoginEmail")   
         menuEmail.SetAttribute("style", "visibility: visible")
         menuEmail.TextContent <- targetUserEmail
     
     let VerifyPermission targetUserPerm =
        // The logged-in user's permission is retrieved from the database,
        // and it is checked whether the admin page is available for the user.
        let statusEmail = JS.Document.GetElementById("StatusChange")
        if targetUserPerm = "2" then            
            statusEmail.SetAttribute("style", "visibility: visible")
        else
            statusEmail.SetAttribute("style", "visibility: hidden")


     let Main () =  
        Server.EnableParallelWrite()
        //Gets the stored email from the session storage and stores it in the userEmail variable
        userEmail := JS.Window.SessionStorage.GetItem("userEmail")         

        if userEmail.Value <> "" && userEmail.Value <> null then
            VerifyUser userEmail.Value            

            async{
                let! perRes = Server.GetUserPermission userEmail.Value
                userPermission := perRes
                VerifyPermission userPermission.Value           
        
            }
            |>Async.StartImmediate

        Templates.MainTemplate.MainForm()           
                      
            .Doc()

     let UserRegistration () =        
        let serverRespons = Var.Create ""           
        
        JS.SetTimeout (fun () ->
            //Gets the stored email from the session storage and stores it in the userEmail variable
            userEmail := JS.Window.SessionStorage.GetItem("userEmail")
            
            if userEmail.Value <> "" && userEmail.Value <> null then
                VerifyUser userEmail.Value   

                async{
                    let! perRes = Server.GetUserPermission userEmail.Value
                    userPermission := perRes

                    VerifyPermission userPermission.Value
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
                    //Checking if all data is filled in
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

            if userEmail.Value <> "" && userEmail.Value <> null then
                VerifyUser userEmail.Value   

                async{
                    let! perRes = Server.GetUserPermission userEmail.Value
                    userPermission := perRes

                    VerifyPermission userPermission.Value
                }
                |>Async.StartImmediate  

        ) 0 |> ignore
                        
        Templates.SignInTemplate.MainForm()
            .OnSubmit(fun e ->
                async{
                    JS.Window.SessionStorage.RemoveItem("userEmail")
                    JS.Window.SessionStorage.RemoveItem("sessionId")

                    userEmail := ""
                    sessionId := ""

                    password := StringValidation.removeForbiddenCharacters e.Vars.Password.Value
                    userEmail := StringValidation.removeForbiddenCharacters e.Vars.Email.Value 

                    //If a user could be found the session id will be stored on the server
                    let! result = Server.LogingInToDatabase password.Value userEmail.Value                    

                    match result with
                    | Some sid ->
                        sessionId := sid           
                        
                        //The session id and user email will be stored in the sessionstorage 
                        //for verifikation
                        JS.Window.SessionStorage.SetItem("sessionId", sid)                            

                        JS.Window.SessionStorage.SetItem("userEmail", userEmail.Value)                                             

                        JS.SetTimeout (fun () ->
                            userEmail := JS.Window.SessionStorage.GetItem("userEmail")

                            VerifyUser userEmail.Value                  

                        ) 0 |> ignore   
                        

                        let! perRes = Server.GetUserPermission userEmail.Value
                        userPermission := perRes

                        VerifyPermission userPermission.Value

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

            if userEmail.Value <> "" && userEmail.Value <> null then
                VerifyUser userEmail.Value   

                async{

                    let! perRes = Server.GetUserPermission userEmail.Value
                    userPermission := perRes

                    VerifyPermission userPermission.Value

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

                    //Fills out the select options
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
                        VerifyUser userEmail.Value   

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

                        //Checking if all data for car registration are filled out
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
                    
                        let verifyValue = isCarDataComplete newCar

                        if verifyValue then
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
                 //The cost of the repair is filled out based on the selected failure
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
        
        //Checking permission and user data
        JS.SetTimeout (fun () ->            
            userEmail := JS.Window.SessionStorage.GetItem("userEmail")

            if userEmail.Value <> "" && userEmail.Value <> null then
                VerifyUser userEmail.Value   

                async {
                    let! perRes = Server.GetUserPermission userEmail.Value
                    userPermission := perRes

                    VerifyPermission userPermission.Value
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
                        //Gets the car data based on the user's email and permission. If the 
                        //logged-in user has permission level 2, all registered car data will 
                        //be added to the list.
                        
                        userEmail := JS.Window.SessionStorage.GetItem("userEmail")
                        let! res = Server.GetCarData userEmail.Value userPermission.Value
                        serverRespons := res.ToString()
                        carData.AppendMany res

                        let! statusOptions = Server.GetStatusNames()                    

                        statusData.AppendMany statusOptions                       
                    
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

        JS.SetTimeout (fun () ->

            userEmail := JS.Window.SessionStorage.GetItem("userEmail")

            if userEmail.Value <> "" && userEmail.Value <> null then
                VerifyUser userEmail.Value   

        ) 0 |> ignore        

        async {     
                
             let! sessionIdResponse = Server.ReturnSessionId()  
             sessionId.Value <- sessionIdResponse
             let sessionChek = JS.Window.SessionStorage.GetItem("sessionId")  

             if sessionChek = sessionId.Value then
                let! currentUser = Server.CurrentUserData userEmail.Value                                              

                //The input fields are filled with the currently logged-in user's data.
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

             VerifyPermission userPermission.Value
             }
             |> Async.StartImmediate

        Templates.UserPage.MainForm()
            .OnClick(fun e ->                
                let allIputs = JS.Document.QuerySelectorAll("input")
                for i in 0 .. allIputs.Length - 1 do
                    let input = allIputs.[i] :?> HTMLInputElement
                    input.RemoveAttribute("disabled")       
                
                //The users data is added to the variable values
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
                    
                    //Checking for empty fields
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
                VerifyUser userEmail.Value   

            async{
                let! statusOptions = Server.GetStatusNames()

                statusData.AppendMany statusOptions

                let opt = JS.Document.CreateElement("option") :?> HTMLOptionElement
                opt.Value <- ""
                opt.Text <- ""
                statusSelect.AppendChild(opt) |> ignore
                
                //The status <option> tag is populated with data.
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

                //The failure <option> tag is populated with data.
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
                    
                    // Enables the buttons and <option> elements
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
                 // Updates the repair cost based on the selected failure
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

                    // If the "Handover" status is selected and the changes are submitted,
                    // the car data will be transferred to the Archive table
                    // and deleted from the other tables.
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
                    //Updates the failure for the car
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
                    //Creates the new failure
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

                    //Gets the car data from the Archive table
                    let! res = Server.GetCarFromArchive e.Vars.searchLicence.Value   

                    carData.AppendMany res   
                    searchEmail:= res.Head.user_id
                    
                    //Disables all the select and submit buttons
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