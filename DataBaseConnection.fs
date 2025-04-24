namespace AlphaProject

open System.Data.SQLite

module DataBaseConnection =
    //let dbFile = "ProjectAlphaDatabase.db"
    let dbFile = "ProjectAlphaDatabase.db"

    let connectToDatabase () =     
        use connection = new SQLiteConnection($"Data Source={dbFile}")
        connection.Open()
        connection

    

