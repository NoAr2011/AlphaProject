import { Bind, Return } from "../WebSharper.StdLib/WebSharper.Concurrency.js"
import AjaxRemotingProvider from "../WebSharper.StdLib/WebSharper.Remoting.AjaxRemotingProvider.js"
import { EncodeJson_UserData, DecodeJson_RepairStatus, DecodeJson_FailureCost, EncodeJson_CarJoinedData, DecodeJson_CarJoinedData, DecodeJson_FSharpOption_1, DecodeJson_UserData } from "./$Generated.js"
import { DecodeList } from "../WebSharper.Web/WebSharper.ClientSideJson.Provider.js"
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
