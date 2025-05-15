import { Bind, Return } from "../WebSharper.StdLib/WebSharper.Concurrency.js"
import AjaxRemotingProvider from "../WebSharper.StdLib/WebSharper.Remoting.AjaxRemotingProvider.js"
import { DecodeList } from "../WebSharper.Web/WebSharper.ClientSideJson.Provider.js"
import { DecodeJson_CarJoinedData, EncodeJson_CarJoinedData, EncodeJson_UserData, DecodeJson_RepairStatus, DecodeJson_FailureCost, DecodeJson_FSharpOption_1, DecodeJson_UserData } from "./$Generated.js"
export function GetCarFromArchive(carLicence){
  return Bind((new AjaxRemotingProvider()).Async("Server/GetCarFromArchive", [carLicence]), (o) => Return(((DecodeList(DecodeJson_CarJoinedData))())(o)));
}
export function GetUserEmail(carLicence){
  return Bind((new AjaxRemotingProvider()).Async("Server/GetUserEmail", [carLicence]), (o) => Return(o));
}
export function DeleteCarFromDatabase(carLicence){
  return Bind((new AjaxRemotingProvider()).Async("Server/DeleteCarFromDatabase", [carLicence]), (o) => Return(o));
}
export function InsertIntoArchive(carData, userEmail){
  return Bind((new AjaxRemotingProvider()).Async("Server/InsertIntoArchive", [(EncodeJson_CarJoinedData())(carData), userEmail]), (o) => Return(o));
}
export function InserNewMalfunction(failureName, failureDesc, failureCost){
  return Bind((new AjaxRemotingProvider()).Async("Server/InserNewMalfunction", [failureName, failureDesc, failureCost]), (o) => Return(o));
}
export function UpdateRepairCost(carLicence, currantCost){
  return Bind((new AjaxRemotingProvider()).Async("Server/UpdateRepairCost", [carLicence, currantCost]), (o) => Return(o));
}
export function UpdateCarMalfunction(carLicence, carMalfunction){
  return Bind((new AjaxRemotingProvider()).Async("Server/UpdateCarMalfunction", [carLicence, carMalfunction]), (o) => Return(o));
}
export function UpdateCarStatus(carLicence, carStatus){
  return Bind((new AjaxRemotingProvider()).Async("Server/UpdateCarStatus", [carLicence, carStatus]), (o) => Return(o));
}
export function UpdateUser(currentUser){
  return Bind((new AjaxRemotingProvider()).Async("Server/UpdateUser", [(EncodeJson_UserData())(currentUser)]), (o) => Return(o));
}
export function GetUserPermission(userEmail){
  return Bind((new AjaxRemotingProvider()).Async("Server/GetUserPermission", [userEmail]), (o) => Return(o));
}
export function GetStatusNames(){
  return Bind((new AjaxRemotingProvider()).Async("Server/GetStatusNames", []), (o) => Return(((DecodeList(DecodeJson_RepairStatus))())(o)));
}
export function GetFailureNames(){
  return Bind((new AjaxRemotingProvider()).Async("Server/GetFailureNames", []), (o) => Return(((DecodeList(DecodeJson_FailureCost))())(o)));
}
export function CurrentUserId(password, email){
  return Bind((new AjaxRemotingProvider()).Async("Server/CurrentUserId", [password, email]), (o) => Return(o));
}
export function InsertCarData(newCar){
  return Bind((new AjaxRemotingProvider()).Async("Server/InsertCarData", [(EncodeJson_CarJoinedData())(newCar)]), (o) => Return(o));
}
export function GetCarByid(carLicence){
  return Bind((new AjaxRemotingProvider()).Async("Server/GetCarByid", [carLicence]), (o) => Return(((DecodeList(DecodeJson_CarJoinedData))())(o)));
}
export function GetCarData(userEMail, userPermission){
  return Bind((new AjaxRemotingProvider()).Async("Server/GetCarData", [userEMail, userPermission]), (o) => Return(((DecodeList(DecodeJson_CarJoinedData))())(o)));
}
export function LogingInToDatabase(userPassword, userEmail){
  return Bind((new AjaxRemotingProvider()).Async("Server/LogingInToDatabase", [userPassword, userEmail]), (o) => Return((DecodeJson_FSharpOption_1())(o)));
}
export function RegisterNewUser(userData){
  return Bind((new AjaxRemotingProvider()).Async("Server/RegisterNewUser", [(EncodeJson_UserData())(userData)]), (o) => Return(o));
}
export function CurrentUserData(userEmail){
  return Bind((new AjaxRemotingProvider()).Async("Server/CurrentUserData", [userEmail]), (o) => Return((DecodeJson_UserData())(o)));
}
export function CurrentUser(password, email){
  return Bind((new AjaxRemotingProvider()).Async("Server/CurrentUser", [password, email]), (o) => Return((DecodeJson_FSharpOption_1())(o)));
}
export function ReturnSessionId(){
  return Bind((new AjaxRemotingProvider()).Async("Server/ReturnSessionId", []), (o) => Return(o));
}
export function EnableParallelWrite(){
  return(new AjaxRemotingProvider()).Send("Server/EnableParallelWrite", []);
}
