namespace AlphaProject

open System.Data.SQLite

module DataBaseConnection =
    
    // let dbFile = "Data Source=ProjectAlphaDatabase.db"

    let dbFile = "Data Source=ProjectAlphaDatabase.db;Version=3;Journal Mode=WAL;Pooling=True;Max Pool Size=100;"

    let connectToDatabase () =     
        use connection = new SQLiteConnection($"Data Source={dbFile}")
        connection.Open()
        connection

    

