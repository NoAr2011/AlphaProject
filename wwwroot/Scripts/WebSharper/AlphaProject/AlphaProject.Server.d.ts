import FSharpAsync from "../WebSharper.StdLib/Microsoft.FSharp.Control.FSharpAsync`1"
import { FSharpList_T } from "../WebSharper.StdLib/Microsoft.FSharp.Collections.FSharpList`1"
export function UpdateCarStatus(carLicence:string, carStatus:string):void
export function GetUserPermission(userEmail:string):FSharpAsync<string>
export function GetStatusNames():FSharpAsync<FSharpList_T<{main_id:BigInt,status_name:string,status_desc:string}>>
export function GetFailureNames():FSharpAsync<FSharpList_T<{main_id:BigInt,failure_name:string,repair_desc:string,repair_costs:number}>>
export function CurrentUserId(password:string, email:string):FSharpAsync<string>
export function InsertCarData(newCar:{car_licence:string,user_id:BigInt,c_type:string,m_year:BigInt,manuf:string,failure:string,repair_costs:number,repair_status:string}):FSharpAsync<string>
export function GetCarData(userEMail:string, userPermission:string):FSharpAsync<FSharpList_T<{car_licence:string,user_id:BigInt,c_type:string,m_year:BigInt,manuf:string,failure:string,repair_costs:number,repair_status:string}>>
export function LogingInToDatabase(userPassword:string, userEmail:string):FSharpAsync<string>
export function RegisterNewUser(userData:{main_id:string,family_name:string,first_name:string,password:string,permission:BigInt,phone_number:string,email:string,city:string,street:string,house_number:string,floor_door:string}):FSharpAsync<string>
export function CurrentUser(password:string, email:string):FSharpAsync<string>
