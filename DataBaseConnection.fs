namespace AlphaProject

open System.Data.SQLite

module DataBaseConnection =
    
    let dbFile = "Data Source=ProjectAlphaDatabase.db"    

    let ConnectToDatabase () =     
        use connection = new SQLiteConnection(dbFile)
        connection.Open()
        use cmd = new SQLiteCommand("PRAGMA journal_mode=WAL;", connection)
        cmd.ExecuteNonQuery() |> ignore

    

