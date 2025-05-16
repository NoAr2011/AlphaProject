namespace AlphaProject
open System.IO
open WebSharper
open WebSharper.Sitelets

module Server =

    let mutable sessions : Map<string, string> = Map.empty

    [<Rpc>]
    let EnableParallelWrite () =
        DataBaseConnection.ConnectToDatabase()

    [<Rpc>]
    let ReturnSessionId()=    
    
        let sessionId =
            if sessions.Count>0 then
                let lastElement = sessions|> Map.toList |> List.head
                fst lastElement       
            else
                "0"
            
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
    let CurrentUserData (userEmail: string) =
        let loginUser = DatabaseSearch.SearchForUsersData (userEmail)
        async {            
                    
            return loginUser
        }

    [<Rpc>]
    let RegisterNewUser (userData: UserData) =           
        let serverResponse = UpdatingDatabase.AddUser(userData)     

        async
            {
            return serverResponse   
        }

    [<Rpc>]
    let LogingInToDatabase (userPassword:string) (userEmail:string) : Async<string option> =  
        let currentUser = System.String(DatabaseSearch.SearchForUsers userPassword userEmail)
        let result = 
            if currentUser <>"No user found" && currentUser <> "" && currentUser <> null then 
                sessions <-Map.empty
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
                    DatabaseSearch.GetAllCarData()
                else
                    DatabaseSearch.GetCarData (userEMail)
            return newCar
        }
    
    [<Rpc>]
    let GetCarByid (carLicence : string) =
        let carData = DatabaseSearch.GetCarByLicence carLicence
        async{
            return carData
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
            | a when a > 0 ->  "Vehicle registration is complete!"
            | _ ->  "This license number already exists."
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
    let UpdateUser (currentUser: UserData) =  
    
        let dataBaseResponse = UpdatingDatabase.UpdateUserData (currentUser)        
        
        async{
            
            return dataBaseResponse
        
        }

    [<Rpc>]
    let UpdateCarStatus (carLicence: string) (carStatus: string) =
        let updateStatus = UpdatingDatabase.UpdateCarStatus carLicence carStatus
        async{
            return updateStatus
        }

    [<Rpc>]
    let UpdateCarMalfunction (carLicence: string) (carMalfunction: string) =
        let updateStatus = UpdatingDatabase.UpdateMalfunction carLicence carMalfunction
        async{
            return updateStatus
        } 

    [<Rpc>]
    let UpdateRepairCost (carLicence: string) (currantCost: float) =
        let updateStatus = UpdatingDatabase.UpdateRepairCost carLicence currantCost
        async{
            return updateStatus
        } 

    [<Rpc>]
    let InserNewMalfunction(failureName, failureDesc, failureCost)=
        let insertStatus = UpdatingDatabase.CreateNewMalfunctions (failureName, failureDesc, failureCost) 
        async{
            return insertStatus
        } 

    [<Rpc>]
    let InsertIntoArchive(carData:CarJoinedData, userEmail: string)=
        let inserIntoArchive = UpdatingDatabase.AddCarToArchive(carData, userEmail)
        async{
            return inserIntoArchive
        }

    [<Rpc>]
    let DeleteCarFromDatabase (carLicence:string) =
        let deletedFromCars = UpdatingDatabase.DeletCarFromDatabase(carLicence)
        let deletedFromFailures = UpdatingDatabase.DeletCarFromFailureSwitch(carLicence)
        let deletedFromstatuses = UpdatingDatabase.DeletCarFromStatusSwitch(carLicence)
        async{
            return deletedFromCars
        }

    [<Rpc>]
    let GetUserEmail(carLicence:string) =
        let userEmail = DatabaseSearch.GetUserByCar(carLicence)        

        async{
        
            return userEmail
        
        }

    [<Rpc>]
    let GetCarFromArchive(carLicence:string) =
        let carData = DatabaseSearch.GetArchiveCar(carLicence)
        async{
            return carData
        }