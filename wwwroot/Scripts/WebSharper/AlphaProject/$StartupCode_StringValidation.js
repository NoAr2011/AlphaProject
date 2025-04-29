import { ofArray } from "../WebSharper.StdLib/Microsoft.FSharp.Collections.ListModule.js"
import { Lazy } from "../WebSharper.Core.JavaScript/Runtime.js"
let _c=Lazy((_i) => class $StartupCode_StringValidation {
  static {
    _c=_i(this);
  }
  static forbiddenChars;
  static {
    this.forbiddenChars=ofArray(["*", "=", "\"", "'", ";"]);
  }
});
export default _c;
