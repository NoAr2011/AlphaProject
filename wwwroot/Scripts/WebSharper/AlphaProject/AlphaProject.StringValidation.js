import { ofSeq } from "../WebSharper.StdLib/Microsoft.FSharp.Collections.ArrayModule.js"
import { filter } from "../WebSharper.StdLib/Microsoft.FSharp.Collections.SeqModule.js"
import { contains } from "../WebSharper.StdLib/Microsoft.FSharp.Collections.ListModule.js"
import $StartupCode_StringValidation from "./$StartupCode_StringValidation.js"
export function removeForbiddenCharacters(input){
  return ofSeq(filter((c) =>!contains(c, forbiddenChars()), input)).join("");
}
export function forbiddenChars(){
  return $StartupCode_StringValidation.forbiddenChars;
}
