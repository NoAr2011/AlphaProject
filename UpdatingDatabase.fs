namespace AlphaProject
open System.Data.SQLite
open System.IO


module UpdatingDatabase = 

    let AddUser (newUser : UserData) =          
        let connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()  
        
        let hashPassword = EncodingData.hashPassword(newUser.password)        
                    
        try           
            
            let insertQuery = "Insert Into User_table (family_name, first_name, password, permissions, phone_number, email, city, street, house_number, floor_door) 
                Values (@family_name, @first_name, @password, @permissions, @phone_number, @email, @city, @street, @house_number, 
                @floor_door)"
            let cmd = new SQLiteCommand(insertQuery, connection)

            
            cmd.Parameters.AddWithValue("@family_name", StringValidation.removeForbiddenCharacters newUser.family_name) |>ignore
            cmd.Parameters.AddWithValue("@first_name", StringValidation.removeForbiddenCharacters newUser.first_name) |>ignore
            cmd.Parameters.AddWithValue("@password",  hashPassword) |>ignore
            cmd.Parameters.AddWithValue("@permissions", newUser.permission) |>ignore
            cmd.Parameters.AddWithValue("@phone_number", StringValidation.removeForbiddenCharacters newUser.phone_number) |>ignore
            cmd.Parameters.AddWithValue("@email", StringValidation.removeForbiddenCharacters newUser.email) |>ignore
            cmd.Parameters.AddWithValue("@city", StringValidation.removeForbiddenCharacters newUser.city) |>ignore
            cmd.Parameters.AddWithValue("@street", StringValidation.removeForbiddenCharacters newUser.street) |>ignore
            cmd.Parameters.AddWithValue("@house_number", StringValidation.removeForbiddenCharacters newUser.house_number) |>ignore
            cmd.Parameters.AddWithValue("@floor_door", StringValidation.removeForbiddenCharacters newUser.floor_door) |>ignore

            cmd.ExecuteNonQuery() |>ignore           
            let returnValue = 1
            connection.Close()
            returnValue 
           
        with
        | ex -> 
            
            let filePath = "errorLog.txt"
            
            File.WriteAllText(filePath, ex.Message)
            let returnValue = 0
            returnValue

    let InsertCarData (newCar :CarJoinedData) =   
        let connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()        
        
        try           
            
            let insertQuery = "Insert Into Cars_table (car_licence, c_type, m_year, manuf, user_id) 
                Values (@car_licence, @c_type, @m_year, @manuf, @user_id)"

            let cmd = new SQLiteCommand(insertQuery, connection)            
            
            cmd.Parameters.AddWithValue("@car_licence", StringValidation.removeForbiddenCharacters newCar.car_licence) |>ignore
            cmd.Parameters.AddWithValue("@c_type", StringValidation.removeForbiddenCharacters newCar.c_type) |>ignore
            cmd.Parameters.AddWithValue("@m_year", newCar.m_year) |>ignore
            cmd.Parameters.AddWithValue("@manuf", StringValidation.removeForbiddenCharacters newCar.manuf) |>ignore
            cmd.Parameters.AddWithValue("@user_id", newCar.user_id) |>ignore           

            cmd.ExecuteNonQuery() |>ignore           
            let returValue = 1
            connection.Close()
            returValue 
           
        with
        | ex -> 
            connection.Close()
            let filePath = "errorLog.txt"
            
            File.WriteAllText(filePath, ex.Message)
            let returnValue = 0
            returnValue
        
    let InsertMalfuncSwitch carLicence (failureId) =                    
        let connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()           

        let insertQuery ="Insert Into Failure_switch (car_licence, failure_id) Values (@car_licence, @failure_id)"
                
        let filePath = "malfLog.txt"
            
        File.WriteAllText(filePath, failureId)

        let cmd = new SQLiteCommand(insertQuery, connection)
        cmd.Parameters.AddWithValue("@car_licence", StringValidation.removeForbiddenCharacters carLicence) |>ignore
        cmd.Parameters.AddWithValue("@failure_id", failureId) |>ignore
        cmd.ExecuteNonQuery() |>ignore           

        connection.Close()       

    let InsertStatusSwitch carLicence (failureId) statusId =                    
        let connection = new SQLiteConnection(DataBaseConnection.dbFile)
        connection.Open()  
        
        let insertQuery = "Insert Into Failure_Status_switch (car_licence, failure_id, status_id) 
        Values(@car_licence, @failure_id, @status_id)"        

        let cmd = new SQLiteCommand(insertQuery, connection)
        cmd.Parameters.AddWithValue("@car_licence", StringValidation.removeForbiddenCharacters carLicence) |>ignore
        cmd.Parameters.AddWithValue("@failure_id", failureId) |>ignore
        cmd.Parameters.AddWithValue("@status_id", statusId) |>ignore
        cmd.ExecuteNonQuery() |>ignore           

        connection.Close()
        