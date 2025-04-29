import Var from "../WebSharper.UI/WebSharper.UI.Var.js"
import { Lazy } from "../WebSharper.Core.JavaScript/Runtime.js"
let _c=Lazy((_i) => class $StartupCode_Client {
  static {
    _c=_i(this);
  }
  static sessionId;
  static password;
  static userEmail;
  static {
    this.userEmail=Var.Create_1("");
    this.password=Var.Create_1("");
    this.sessionId=Var.Create_1("");
  }
});
export default _c;
