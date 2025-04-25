import Object from "../WebSharper.StdLib/System.Object.js"
import { Const } from "./WebSharper.UI.Anim.js"
import { Equals } from "../WebSharper.StdLib/Microsoft.FSharp.Core.Operators.Unchecked.js"
export default class Trans extends Object {
  change;
  enter;
  exit;
  flags;
  Copy(change, enter, exit, flags){
    const ch=change==null?(_1) =>(_2) => this.TChange(_1, _2):change.$0;
    const d=this.TEnter;
    const d_1=this.TExit;
    const d_2=this.TFlags;
    return new Trans("New_3", (d_3, d_4) =>(ch(d_3))(d_4), enter==null?d:enter.$0, exit==null?d_1:exit.$0, flags==null?d_2:flags.$0);
  }
  get TFlags(){
    return this.flags;
  }
  get TExit(){
    return this.exit;
  }
  get TEnter(){
    return this.enter;
  }
  TChange(x, y){
    return this.change(x, y);
  }
  static New(ch, enter, exit){
    return new this("New", ch, enter, exit);
  }
  static New_1(ch){
    return new this("New_1", ch);
  }
  static New_2(){
    return new this("New_2");
  }
  static New_3(change, enter, exit, flags){
    return new this("New_3", change, enter, exit, flags);
  }
  constructor(i, _1, _2, _3, _4){
    if(i=="New_2"){
      i="New_3";
      _1=(_5, y) => Const(y);
      _2=Const;
      _3=Const;
      _4=0;
    }
    let ch;
    if(i=="New_1"){
      ch=_1;
      i="New_3";
      _1=ch;
      _2=Const;
      _3=Const;
      _4=1;
    }
    let ch_1;
    let enter;
    let exit;
    if(i=="New"){
      ch_1=_1;
      enter=_2;
      exit=_3;
      i="New_3";
      _1=ch_1;
      _2=Equals(enter, null)?Const:enter;
      _3=Equals(exit, null)?Const:exit;
      _4=1|(Equals(enter, null)?0:2)|(Equals(exit, null)?0:4);
    }
    if(i=="New_3"){
      const change=_1;
      const enter_1=_2;
      const exit_1=_3;
      const flags=_4;
      super();
      this.change=change;
      this.enter=enter_1;
      this.exit=exit_1;
      this.flags=flags;
    }
  }
}
