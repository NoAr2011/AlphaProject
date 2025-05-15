import { DecodeRecord, Id, DecodeBigInteger, EncodeRecord, EncodeBigInteger, DecodeUnion } from "../WebSharper.Web/WebSharper.ClientSideJson.Provider.js"
import { LoadLocalTemplates, NamedTemplate } from "../WebSharper.UI/WebSharper.UI.Client.Templates.js"
import { Some } from "../WebSharper.StdLib/Microsoft.FSharp.Core.FSharpOption`1.js"
let Decoder_CarJoinedData;
let Encoder_CarJoinedData;
let Encoder_UserData;
let Decoder_RepairStatus;
let Decoder_FailureCost;
let Decoder_FSharpOption_1;
let Decoder_UserData;
export function DecodeJson_CarJoinedData(){
  return Decoder_CarJoinedData?Decoder_CarJoinedData:Decoder_CarJoinedData=(DecodeRecord(void 0, [["car_licence", Id(), 0], ["user_id", Id(), 0], ["c_type", Id(), 0], ["m_year", DecodeBigInteger(), 0], ["manuf", Id(), 0], ["failure", Id(), 0], ["repair_costs", Id(), 0], ["repair_status", Id(), 0]]))();
}
export function EncodeJson_CarJoinedData(){
  return Encoder_CarJoinedData?Encoder_CarJoinedData:Encoder_CarJoinedData=(EncodeRecord(void 0, [["car_licence", Id(), 0], ["user_id", Id(), 0], ["c_type", Id(), 0], ["m_year", EncodeBigInteger(), 0], ["manuf", Id(), 0], ["failure", Id(), 0], ["repair_costs", Id(), 0], ["repair_status", Id(), 0]]))();
}
export function EncodeJson_UserData(){
  return Encoder_UserData?Encoder_UserData:Encoder_UserData=(EncodeRecord(void 0, [["main_id", Id(), 0], ["family_name", Id(), 0], ["first_name", Id(), 0], ["password", Id(), 0], ["permission", EncodeBigInteger(), 0], ["phone_number", Id(), 0], ["email", Id(), 0], ["city", Id(), 0], ["street", Id(), 0], ["house_number", Id(), 0], ["floor_door", Id(), 0]]))();
}
export function DecodeJson_RepairStatus(){
  return Decoder_RepairStatus?Decoder_RepairStatus:Decoder_RepairStatus=(DecodeRecord(void 0, [["main_id", DecodeBigInteger(), 0], ["status_name", Id(), 0], ["status_desc", Id(), 0]]))();
}
export function DecodeJson_FailureCost(){
  return Decoder_FailureCost?Decoder_FailureCost:Decoder_FailureCost=(DecodeRecord(void 0, [["main_id", DecodeBigInteger(), 0], ["failure_name", Id(), 0], ["repair_desc", Id(), 0], ["repair_costs", Id(), 0]]))();
}
export function DecodeJson_FSharpOption_1(){
  return Decoder_FSharpOption_1?Decoder_FSharpOption_1:Decoder_FSharpOption_1=(DecodeUnion(void 0, "$", [null, [1, [["$0", "Value", Id(), 0]]]]))();
}
export function DecodeJson_UserData(){
  return Decoder_UserData?Decoder_UserData:Decoder_UserData=(DecodeRecord(void 0, [["main_id", Id(), 0], ["family_name", Id(), 0], ["first_name", Id(), 0], ["password", Id(), 0], ["permission", DecodeBigInteger(), 0], ["phone_number", Id(), 0], ["email", Id(), 0], ["city", Id(), 0], ["street", Id(), 0], ["house_number", Id(), 0], ["floor_door", Id(), 0]]))();
}
export function mainform(h){
  LoadLocalTemplates("changingcarstatus");
  return h?NamedTemplate("changingcarstatus", Some("mainform"), h):void 0;
}
export function listitem(h){
  LoadLocalTemplates("changingcarstatus");
  return h?NamedTemplate("changingcarstatus", Some("listitem"), h):void 0;
}
export function mainform_1(h){
  LoadLocalTemplates("userdatapage");
  return h?NamedTemplate("userdatapage", Some("mainform"), h):void 0;
}
export function listitem_1(h){
  LoadLocalTemplates("carstatus");
  return h?NamedTemplate("carstatus", Some("listitem"), h):void 0;
}
export function mainform_2(h){
  LoadLocalTemplates("carstatus");
  return h?NamedTemplate("carstatus", Some("mainform"), h):void 0;
}
export function mainform_3(h){
  LoadLocalTemplates("carregist");
  return h?NamedTemplate("carregist", Some("mainform"), h):void 0;
}
export function mainform_4(h){
  LoadLocalTemplates("loginpage");
  return h?NamedTemplate("loginpage", Some("mainform"), h):void 0;
}
export function mainform_5(h){
  LoadLocalTemplates("userregist");
  return h?NamedTemplate("userregist", Some("mainform"), h):void 0;
}
export function mainform_6(h){
  LoadLocalTemplates("main");
  return h?NamedTemplate("main", Some("mainform"), h):void 0;
}
