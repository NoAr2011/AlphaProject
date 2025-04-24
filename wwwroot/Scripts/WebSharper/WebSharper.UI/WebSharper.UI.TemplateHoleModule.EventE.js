import TemplateHole from "./WebSharper.UI.TemplateHole.js"
export default class EventE extends TemplateHole {
  name;
  key;
  fillWith;
  Key(){
    return this.key;
  }
  get Value(){
    return this.fillWith;
  }
  WithName(n){
    return new EventE(n, this.key, this.fillWith);
  }
  get ValueObj(){
    return this.Value;
  }
  get Name(){
    return this.name;
  }
  constructor(name, key, fillWith){
    super();
    this.name=name;
    this.key=key;
    this.fillWith=fillWith;
  }
}
