namespace AlphaProject
open System.IO
open System.Data.SQLite


module DatabaseSearch =     

    let SearchForUsers (password : string) email : string = 
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()       
        
        let hashPassword = EncodingData.hashPassword(password) 

        let sqlQuery = $"Select first_name from User_table Where password = @password and email = @email"
               

        use cmd = new SQLiteCommand(sqlQuery, connection)   
        cmd.Parameters.AddWithValue("@password", hashPassword) |> ignore
        cmd.Parameters.AddWithValue("@email", StringValidation.removeForbiddenCharacters email) |> ignore
        let reader = cmd.ExecuteScalar()                           
       
        try
            
            let currentUser = 
                match reader with
                | null -> "No user found"
                | _ -> reader.ToString()      

            connection.Close()

            currentUser
        with
        | ex -> 
            connection.Close()
            let filePath = "errorLog.txt"
            
            File.WriteAllText(filePath, ex.Message)
            let returnValue = ""

            returnValue

         
    let GetCarData userEMail =        
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()        
        
          
        let sqlQuery = $"""Select u.main_id, c.car_licence, c.c_type, c.m_year, c.manuf, 
                f.failure_name, c.repair_cost, s.status_name, s.status_desc From
                User_table u Inner Join Cars_table c On u.main_id = c.user_id 
                Left Join Failure_switch fs On fs.car_licence = c.car_licence 
                Left Join Failure_costs f On f.main_id = fs.failure_id 
                Left Join Failure_status_switch fss On fss.car_licence = c.car_licence 
                Left Join Repair_statuses s On s.main_id = fss.status_id Where 
                u.email = @email"""       

        use cmd = new SQLiteCommand(sqlQuery, connection)  
        cmd.Parameters.AddWithValue("@email", StringValidation.removeForbiddenCharacters userEMail) |> ignore
        let reader = cmd.ExecuteReader()

        let rec GetDataTable tableData=       
      
            if reader.Read() then           

                let currentCar : CarJoinedData= 
                    {

                    car_licence = reader.GetString(1)                     
                    c_type = reader.GetString(2)
                    m_year = reader.GetInt64(3)
                    user_id = reader.GetInt64(0).ToString()
                    manuf = reader.GetString(4)
                    failure = reader.GetString(5)
                    repair_costs =reader.GetValue(6) :?> float
                    repair_status = reader.GetString(7)

                }
                                                
                let tempList = [currentCar]
                let return_list = tableData @ tempList

                GetDataTable return_list
                
            else
                tableData 
                    
        let emptyCars = []

        let allCars = GetDataTable emptyCars                     
        connection.Close()
        allCars
        

    let GetUserId email : string =     
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()        

        let sqlQuery = $"Select main_id from User_table Where email = @email"

        use cmd = new SQLiteCommand(sqlQuery, connection)    
        cmd.Parameters.AddWithValue("@email", StringValidation.removeForbiddenCharacters email) |> ignore
        let reader = cmd.ExecuteScalar()                          

        let currentUserId = reader.ToString()     

        connection.Close()

        currentUserId
    
    let GetUserByCar(carLicence:string)=
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()        

        let sqlQuery = $"Select email from User_table Where main_id = (Select user_id from Cars_table where car_licence = @carLicence)"

        use cmd = new SQLiteCommand(sqlQuery, connection)    
        cmd.Parameters.AddWithValue("@carLicence", carLicence) |> ignore
        let reader = cmd.ExecuteScalar()                          

        let userEmail =
            if isNull reader then
                "Nincs találat"
            else
                reader.ToString()   

        connection.Close()

        userEmail

    let GetUserEmail (userId : int64) =     
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()        

        let sqlQuery = $"Select email from User_table Where main_id = @userId"

        use cmd = new SQLiteCommand(sqlQuery, connection)    
        cmd.Parameters.AddWithValue("@userId", userId) |> ignore
        let reader = cmd.ExecuteScalar()                          

        let userEmail = reader.ToString()     

        connection.Close()

        userEmail

    let GetMalfunctions () =     
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()        

        let sqlQuery = "Select * from Failure_costs"               

        use cmd = new SQLiteCommand(sqlQuery, connection)        
        let reader = cmd.ExecuteReader()                       

        let rec GetDataTable tableList =      
            if reader.Read() then  
                let currentFailure =
                    {
                    main_id = reader.GetInt64(0)
                    failure_name = reader.GetString(1)
                    repair_desc = reader.GetString(2)
                    repair_costs = float(reader.GetFloat(3))
                    
                }               

                GetDataTable (currentFailure :: tableList)
            else
                tableList

        let emptyList = []

        let failureList = GetDataTable emptyList 
        connection.Close()
        failureList

    let GetcarStatuses () = 
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()        

        let sqlQuery = "Select * from Repair_statuses"
               

        use cmd = new SQLiteCommand(sqlQuery, connection)        
        let reader = cmd.ExecuteReader()                       

        let rec GetDataTable tableList =      
            if reader.Read() then  
                let currentStatus =
                    {
                    main_id = reader.GetInt64(0)
                    status_name = reader.GetString(1)
                    status_desc = reader.GetString(2)                    
                    
                }               

                GetDataTable (currentStatus :: tableList)
            else
                tableList

        let emptyList = []

        let statusList = GetDataTable emptyList 
        connection.Close()
        statusList

    let GetMainIdFromTable tableName (columnName) searchedBy =
       use connection = new SQLiteConnection(DataBaseConnection.dbFile)
       connection.Open()        

       let sqlQuery = $"Select main_id from {tableName} Where {columnName} = @searched"

       use cmd = new SQLiteCommand(sqlQuery, connection)  
       cmd.Parameters.AddWithValue("@searched", searchedBy) |> ignore
       let reader = cmd.ExecuteScalar()                          

       let mainId = reader.ToString()    

       connection.Close()

       mainId

    let GetPermision (userEMail:string) : string =        
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()    

        let sqlQuery = $"Select permissions from User_table Where email = @Email"

        use cmd = new SQLiteCommand(sqlQuery, connection)  
        cmd.Parameters.AddWithValue("@Email", StringValidation.removeForbiddenCharacters userEMail) |> ignore
        let reader = cmd.ExecuteScalar()                          
        
        let userPerm = reader.ToString() 


        connection.Close()

        userPerm

    let GetAllCarData () =        
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()        
        
          
        let sqlQuery = $"""SELECT u.main_id, c.car_licence, c.c_type, c.m_year, c.manuf, 
                f.failure_name, c.repair_cost, s.status_name, s.status_desc FROM
                User_table u INNER JOIN Cars_table c ON u.main_id = c.user_id 
                LEFT JOIN Failure_switch fs ON fs.car_licence = c.car_licence 
                LEFT JOIN Failure_costs f ON f.main_id = fs.failure_id 
                LEFT JOIN Failure_status_switch fss ON fss.car_licence = c.car_licence 
                LEFT JOIN Repair_statuses s ON s.main_id = fss.status_id"""       

        use cmd = new SQLiteCommand(sqlQuery, connection)  
        
        let reader = cmd.ExecuteReader()

        let rec GetDataTable tableData=       
      
            if reader.Read() then           

                let currentCar : CarJoinedData= 
                    {

                    car_licence = reader.GetString(1)                     
                    c_type = reader.GetString(2)
                    m_year = reader.GetInt64(3)
                    user_id = reader.GetInt64(0).ToString()
                    manuf = reader.GetString(4)
                    failure = reader.GetString(5)
                    repair_costs =reader.GetValue(6) :?> float
                    repair_status = reader.GetString(7)

                }
                                                
                let tempList = [currentCar]
                let returnList = tableData @ tempList

                GetDataTable returnList
                
            else
                tableData 
                    
        let emptyCars = []

        let allCars = GetDataTable emptyCars                     
        connection.Close()
        allCars

    let SearchForUsersData (email : string) : UserData = 
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()       

        let sqlQuery = $"Select family_name, first_name, phone_number, city, street, house_number, floor_door from User_table Where email = @Email"

        use cmd = new SQLiteCommand(sqlQuery, connection)  
        cmd.Parameters.AddWithValue("@Email", StringValidation.removeForbiddenCharacters email) |> ignore
        let reader = cmd.ExecuteReader()                        
        
        let currentUser : UserData =
            if reader.Read() then
                {
                main_id = ""
                family_name = reader.GetString(0)
                first_name = reader.GetString(1)    
                password = ""
                permission = 4
                phone_number = reader.GetString(2)
                email = ""
                city = reader.GetString(3)
                street = reader.GetString(4)
                house_number = reader.GetString(5)
                floor_door = reader.GetString(6)    
               }
            else                
                failwith "Nincs ilyen felhasználó az adatbázisban"

        connection.Close()
        currentUser
        

    let GetCarByLicence carLicence =        
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()       
                  
        let sqlQuery = $"""Select u.main_id, c.car_licence, c.c_type, c.m_year, c.manuf, 
                f.failure_name, c.repair_cost, s.status_name, s.status_desc From
                User_table u Inner Join Cars_table c On u.main_id = c.user_id 
                Left Join Failure_switch fs On fs.car_licence = c.car_licence  
                Left Join Failure_costs f On f.main_id = fs.failure_id 
                Left Join Failure_status_switch fss On fss.car_licence = c.car_licence 
                Left Join Repair_statuses s On s.main_id = fss.status_id Where 
                c.car_licence = @carLicence"""       

        use cmd = new SQLiteCommand(sqlQuery, connection)  
        cmd.Parameters.AddWithValue("@carLicence", StringValidation.removeForbiddenCharacters carLicence) |> ignore
        let reader = cmd.ExecuteReader()

        let rec GetDataTable tableData=       
      
            if reader.Read() then           

                let currentCar : CarJoinedData= 
                    {

                    car_licence = reader.GetString(1)                     
                    c_type = reader.GetString(2)
                    m_year = reader.GetInt64(3)
                    user_id = reader.GetInt64(0).ToString()
                    manuf = reader.GetString(4)
                    failure = reader.GetString(5)
                    repair_costs =reader.GetValue(6) :?> float
                    repair_status = reader.GetString(7)

                }
                                                
                let tempList = [currentCar]
                let return_list = tableData @ tempList

                GetDataTable return_list
                
            else
                tableData 
                    
        let emptyCars = []

        let allCars = GetDataTable emptyCars                     
        connection.Close()        
        allCars   

    let GetArchiveCar carLicence =        
        use connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()       
                  
        let sqlQuery = "Select * from Archive Where car_licence = @carLicence"      

        use cmd = new SQLiteCommand(sqlQuery, connection)  
        cmd.Parameters.AddWithValue("@carLicence", StringValidation.removeForbiddenCharacters carLicence) |> ignore
        let reader = cmd.ExecuteReader()

        let rec GetDataTable tableData=       
      
            if reader.Read() then           

                let currentCar : CarJoinedData= 
                    {

                    car_licence = reader.GetString(1)                     
                    c_type = reader.GetString(2)
                    m_year = reader.GetInt64(3)                    
                    manuf = reader.GetString(4)
                    failure = reader.GetString(5)
                    repair_costs =reader.GetValue(6) :?> float
                    repair_status = ""
                    user_id = reader.GetString(7)

                }
                                                
                let tempList = [currentCar]
                let return_list = tableData @ tempList

                GetDataTable return_list
                
            else
                tableData 
                    
        let emptyCars = []

        let allCars = GetDataTable emptyCars                     
        connection.Close()        
        allCars   