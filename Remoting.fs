namespace AlphaProject
open System.IO
open WebSharper
open WebSharper.Sitelets

module Server =

    let mutable sessions : Map<string, string> = Map.empty

    [<Rpc>]
    let ReturnSessionId()=        
        let lastElement = sessions|> Map.toList |> List.head
        let sessionId = fst lastElement       

        async{
            return sessionId 
        }

    [<Rpc>]
    let CurrentUser (password : string) (email : string): Async<string option>=   
        let loginUser = (DatabaseSearch.SearchForUsers password email).ToString()
        let result = 
            if loginUser <>"No user found" && loginUser <> "" && loginUser <> null then 
                
                let sessionId = System.Guid.NewGuid().ToString()
                sessions <- sessions.Add(sessionId, email)

                Some sessionId

            else
                None

        async {
            
                    
            return result
        }

    [<Rpc>]
    let RegisterNewUser (userData: UserData) =           
        let serverResponse = UpdatingDatabase.AddUser(userData)       
        
        let returnValue =
            match serverResponse with                    
            | a when a > 0 ->  "Your registration is complete!"
            | _ ->  "This email address already exists.!"   

        async
            {
            return returnValue   
        }

    [<Rpc>]
    let LogingInToDatabase (userPassword:string) (userEmail:string) : Async<string option> =  
        let currentUser = System.String(DatabaseSearch.SearchForUsers userPassword userEmail)
        let result = 
            if currentUser <>"No user found" && currentUser <> "" && currentUser <> null then 
                
                let sessionId = System.Guid.NewGuid().ToString()
                sessions <- sessions.Add(sessionId, userEmail)
                Some sessionId

             else
                None        

        async {            
                    
            return result           
           
        }

    [<Rpc>]
    let GetCarData (userEMail : string) (userPermission : string) = 

        async {
            let newCar =
                if userPermission = "2" then
                    DatabaseSearch.GetAllCarData
                else
                    DatabaseSearch.GetCarData userEMail
            return newCar
        }
    
    [<Rpc>]
    let InsertCarData (newCar: CarJoinedData) =   
        
        let serverResponse = UpdatingDatabase.InsertCarData(newCar)       

        if serverResponse > 0 then
            let failureMainId = DatabaseSearch.GetMainIdFromTable "Failure_costs" "failure_name" newCar.failure        
        
            UpdatingDatabase.InsertMalfuncSwitch newCar.car_licence failureMainId
        
            UpdatingDatabase.InsertStatusSwitch newCar.car_licence failureMainId newCar.repair_status       

        let returnValue =
            match serverResponse with                    
            | a when a > 0 ->  "Your registration is complete!"
            | _ ->  "Something went wrong!"
        async
            {
            return returnValue   
        }

    [<Rpc>]
    let CurrentUserId (password : string) (email : string) : Async<string>=    
        let userId = (DatabaseSearch.GetUserId email).ToString()
        
        async {
            
            return userId
        }

    [<Rpc>]
    let GetFailureNames () =        
        let failureList = DatabaseSearch.GetMalfunctions()
        
        async {
            
            return failureList
        }

    [<Rpc>]
    let GetStatusNames ()=
        let statusList = DatabaseSearch.GetcarStatuses()       

        async{
            return statusList
        }

    [<Rpc>]
    let GetUserPermission (userEmail: string) : Async<string> =
        let userPerm = DatabaseSearch.GetPermision userEmail
        async{
            return userPerm
        }

    [<Rpc>]
    let UpdateCarStatus (carLicence: string) (carStatus: string) =
        let filePath = "statusLog.txt"
        File.WriteAllText(filePath, carLicence)
        File.AppendAllText(filePath, carStatus)