namespace AlphaProject
open WebSharper.UI



type UserData = 
     {
     main_id: string
     family_name : string
     first_name : string     
     password : string
     permission: int64
     phone_number : string
     email : string     
     city: string
     street : string
     house_number : string
     floor_door : string    
     
    }

type UersData = ListModel<string, UserData>

type Car = 
        {
        car_licence : string
        c_type : string
        m_year : int64
        manuf : string      
        user_id : string 
        repair_costs : float
    }  

type FailureSwitch =
        {
        main_id : int64
        car_licence : string  
        failure_id : int       
    }

type FailureCost =
        {
        main_id : int64 
        failure_name : string
        repair_desc : string
        repair_costs : float        
    }

type FailureCosts = ListModel<string, FailureCost>

type RepairStatus =
        {
        main_id : int64
        status_name : string     
        status_desc : string
    }

type RepairStatuses = ListModel<string, RepairStatus>

type Failure_switch =
        {
        main_id : int64    
        car_licence : string     
        failure_id : int64   
    
    }

type CarJoinedData =
        {
        car_licence : string       
        user_id : string
        c_type : string
        m_year : int64
        manuf : string  
        failure : string
        repair_costs : float
        repair_status: string

    }

type CarsJoinedData = ListModel<string, CarJoinedData>

type CarData = 
    {

    car_licence : string
    user_id : string
    c_type : string
    m_year : int64
    manuf : string      
    
}

type CarsData = ListModel<string, CarData>

