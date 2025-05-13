import { Create } from "../WebSharper.UI/WebSharper.UI.ListModel.js"
import FSharpList from "../WebSharper.StdLib/Microsoft.FSharp.Collections.FSharpList`1.js"
import Var from "../WebSharper.UI/WebSharper.UI.Var.js"
import { StartImmediate, Delay, Bind, For, Zero, Combine } from "../WebSharper.StdLib/WebSharper.Concurrency.js"
import { GetStatusNames, ReturnSessionId, GetCarByid, UpdateCarStatus, CurrentUserData, GetUserPermission, UpdateUser, GetCarData, GetFailureNames, CurrentUserId, InsertCarData, LogingInToDatabase, RegisterNewUser } from "./AlphaProject.Server.js"
import Doc from "../WebSharper.UI/WebSharper.UI.Doc.js"
import ProviderBuilder from "../WebSharper.UI.Templating.Runtime/WebSharper.UI.Templating.Runtime.Server.ProviderBuilder.js"
import Text from "../WebSharper.UI/WebSharper.UI.TemplateHoleModule.Text.js"
import { CompleteHoles, EventQ2 } from "../WebSharper.UI.Templating.Runtime/WebSharper.UI.Templating.Runtime.Server.Handler.js"
import TemplateInstance from "../WebSharper.UI.Templating.Runtime/WebSharper.UI.Templating.Runtime.Server.TemplateInstance.js"
import { listitem, mainform, mainform_1, mainform_2, mainform_3, mainform_4, mainform_5, mainform_6 } from "./$Generated.js"
import TemplateHole from "../WebSharper.UI/WebSharper.UI.TemplateHole.js"
import Elt from "../WebSharper.UI/WebSharper.UI.TemplateHoleModule.Elt.js"
import { removeForbiddenCharacters } from "./AlphaProject.StringValidation.js"
import { IsNullOrWhiteSpace } from "../WebSharper.StdLib/Microsoft.FSharp.Core.StringModule.js"
import { Get } from "../WebSharper.StdLib/WebSharper.Enumerator.js"
import { isIDisposable } from "../WebSharper.StdLib/System.IDisposable.js"
import { tryFind } from "../WebSharper.StdLib/Microsoft.FSharp.Collections.SeqModule.js"
import $StartupCode_Client from "./$StartupCode_Client.js"
export function ChangeStatus(){
  const carData=Create((item) => item.car_licence, FSharpList.Empty);
  Var.Create_1("");
  const statusData=Create((item) => item.status_name, FSharpList.Empty);
  setTimeout(() => {
    const faulureSelect=globalThis.document.getElementById("status");
    userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
    if(userEmail().Get()!=""&&userEmail().Get()!=null){
      const menuEmail=globalThis.document.getElementById("LoginEmail");
      menuEmail.setAttribute("style", "visibility: visible");
      menuEmail.textContent=userEmail().Get();
    }
    const _2=null;
    StartImmediate(Delay(() => Bind(GetStatusNames(), (a) => {
      statusData.AppendMany(a);
      return For(statusData, (a_1) => {
        const opt=globalThis.document.createElement("option");
        opt.value=a_1.status_name;
        opt.text=a_1.status_name;
        faulureSelect.appendChild(opt);
        return Zero();
      });
    })), null);
  }, 0);
  const L=Doc.Convert((item) => {
    const this_2=new ProviderBuilder("New_1");
    const this_3=(this_2.h.push(new Text("license", item.car_licence)),this_2);
    const this_4=(this_3.h.push(new Text("manuf", item.manuf)),this_3);
    const this_5=(this_4.h.push(new Text("c_type", item.c_type)),this_4);
    const this_6=(this_5.h.push(new Text("m_year", String(item.m_year))),this_5);
    const this_7=(this_6.h.push(new Text("failure", item.failure)),this_6);
    const this_8=(this_7.h.push(new Text("repair_cost", String(item.repair_costs))),this_7);
    const b_1=(this_8.h.push(new Text("repair_status", item.repair_status)),this_8);
    const p_1=CompleteHoles(b_1.k, b_1.h, []);
    const i_1=new TemplateInstance(p_1[1], listitem(p_1[0]));
    let _2=(b_1.i=i_1,i_1);
    return _2.Doc;
  }, carData.v);
  const t=new ProviderBuilder("New_1");
  const this_1=(t.h.push(EventQ2(t.k, "onclick", () => t.i, (e) => {
    const _2=null;
    StartImmediate(Delay(() => Bind(ReturnSessionId(), (a) => {
      sessionId().Set(a);
      return globalThis.sessionStorage.getItem("sessionId")==sessionId().Get()?Bind(GetCarByid(TemplateHole.Value(e.Vars.Hole("licence")).Get()), (a_1) => {
        carData.AppendMany(a_1);
        return Zero();
      }):Zero();
    })), null);
  })),t);
  const t_1=(this_1.h.push(new Elt("listcontainer", L)),this_1);
  const b=(t_1.h.push(EventQ2(t_1.k, "onsubmit", () => t_1.i, (e) => {
    const _2=null;
    StartImmediate(Delay(() => {
      const currentStatus=TemplateHole.Value(e.Vars.Hole("status")).Get();
      return Bind(UpdateCarStatus(TemplateHole.Value(e.Vars.Hole("licence")).Get(), currentStatus), (a) => {
        alert(String(a));
        globalThis.location.reload();
        return Zero();
      });
    }), null);
  })),t_1);
  const p=CompleteHoles(b.k, b.h, [["licence", 0, null], ["status", 0, null], ["repair_cost", 1, null]]);
  const i=new TemplateInstance(p[1], mainform(p[0]));
  let _1=(b.i=i,i);
  return _1.Doc;
}
export function UserDataPage(){
  const serverRespons=Var.Create_1("");
  const userPermission=Var.Create_1("");
  setTimeout(() => {
    userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
    if(userEmail().Get()!=""&&userEmail().Get()!=null){
      const menuEmail=globalThis.document.getElementById("LoginEmail");
      menuEmail.setAttribute("style", "visibility: visible");
      menuEmail.textContent=userEmail().Get();
    }
  }, 0);
  const _1=null;
  StartImmediate(Delay(() => Bind(ReturnSessionId(), (a) => {
    sessionId().Set(a);
    return Combine(globalThis.sessionStorage.getItem("sessionId")==sessionId().Get()?Bind(CurrentUserData(userEmail().Get()), (a_1) => {
      setTimeout(() => {
        const inputs=globalThis.document.querySelectorAll("input");
        inputs[0].value=a_1.first_name;
        inputs[1].value=a_1.family_name;
        inputs[2].value=a_1.phone_number;
        inputs[3].value=a_1.city;
        inputs[4].value=a_1.street;
        inputs[5].value=a_1.house_number;
        inputs[6].value=a_1.floor_door;
      }, 0);
      return Zero();
    }):Zero(), Delay(() => Bind(GetUserPermission(userEmail().Get()), (a_1) => {
      userPermission.Set(a_1);
      return userPermission.Get()=="2"?(globalThis.document.getElementById("StatusChange").setAttribute("style", "visibility: visible"),Zero()):Zero();
    })));
  })), null);
  const t=new ProviderBuilder("New_1");
  const t_1=(t.h.push(EventQ2(t.k, "onclick", () => t.i, (e) => {
    const allIputs=globalThis.document.querySelectorAll("input");
    for(let i_1=0, _3=allIputs.length-1;i_1<=_3;i_1++)allIputs[i_1].removeAttribute("disabled");
    TemplateHole.Value(e.Vars.Hole("firstname")).Set(allIputs[0].value);
    TemplateHole.Value(e.Vars.Hole("lastname")).Set(allIputs[1].value);
    TemplateHole.Value(e.Vars.Hole("phone")).Set(allIputs[2].value);
    TemplateHole.Value(e.Vars.Hole("city")).Set(allIputs[3].value);
    TemplateHole.Value(e.Vars.Hole("street")).Set(allIputs[4].value);
    TemplateHole.Value(e.Vars.Hole("housenumber")).Set(allIputs[5].value);
    TemplateHole.Value(e.Vars.Hole("floordoor")).Set(allIputs[6].value);
  })),t);
  const b=(t_1.h.push(EventQ2(t_1.k, "onsubmit", () => t_1.i, (e) => {
    const _3=null;
    StartImmediate(Delay(() => {
      const f=removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("firstname")).Get());
      const f_1=removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("lastname")).Get());
      const e_1=userEmail().Get();
      const newUser={
        main_id:"notchanged", 
        family_name:f_1, 
        first_name:f, 
        password:"notchanged", 
        permission:BigInt(4), 
        phone_number:removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("phone")).Get()), 
        email:e_1, 
        city:removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("city")).Get()), 
        street:removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("street")).Get()), 
        house_number:removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("housenumber")).Get()), 
        floor_door:removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("floordoor")).Get())
      };
      const isNonEmpty=(s) =>!IsNullOrWhiteSpace(s);
      let _4=isNonEmpty(newUser.family_name)&&isNonEmpty(newUser.first_name)&&isNonEmpty(newUser.phone_number)&&isNonEmpty(newUser.email)&&isNonEmpty(newUser.city)&&isNonEmpty(newUser.street)&&isNonEmpty(newUser.house_number)&&isNonEmpty(newUser.floor_door)?Bind(UpdateUser(newUser), (a) => {
        serverRespons.Set(String(a));
        return Zero();
      }):(serverRespons.Set("Please fill in all fields."),Zero());
      return Combine(_4, Delay(() => {
        alert(serverRespons.Get());
        globalThis.location.reload();
        return Zero();
      }));
    }), null);
  })),t_1);
  const p=CompleteHoles(b.k, b.h, [["firstname", 0, null], ["lastname", 0, null], ["phone", 0, null], ["city", 0, null], ["street", 0, null], ["housenumber", 0, null], ["floordoor", 0, null]]);
  const i=new TemplateInstance(p[1], mainform_1(p[0]));
  let _2=(b.i=i,i);
  return _2.Doc;
}
export function CarStatus(){
  const carData=Create((item) => item.car_licence, FSharpList.Empty);
  const statusData=Create((item) => item.status_name, FSharpList.Empty);
  const serverRespons=Var.Create_1("");
  const userPermission=Var.Create_1("");
  setTimeout(() => {
    userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
    if(userEmail().Get()!=""&&userEmail().Get()!=null){
      const menuEmail=globalThis.document.getElementById("LoginEmail");
      menuEmail.setAttribute("style", "visibility: visible");
      menuEmail.textContent=userEmail().Get();
    }
    if(userEmail().Get()==""||userEmail().Get()==null)userPermission.Set("4");
    else {
      const _2=null;
      StartImmediate(Delay(() => Bind(GetUserPermission(userEmail().Get()), (a) => {
        userPermission.Set(a);
        return userPermission.Get()=="2"?(globalThis.document.getElementById("StatusChange").setAttribute("style", "visibility: visible"),Zero()):Zero();
      })), null);
    }
  }, 0);
  const L=Doc.Convert((item) => {
    const this_2=new ProviderBuilder("New_1");
    const this_3=(this_2.h.push(new Text("license", item.car_licence)),this_2);
    const this_4=(this_3.h.push(new Text("manuf", item.manuf)),this_3);
    const this_5=(this_4.h.push(new Text("c_type", item.c_type)),this_4);
    const this_6=(this_5.h.push(new Text("m_year", String(item.m_year))),this_5);
    const this_7=(this_6.h.push(new Text("failure", item.failure)),this_6);
    const this_8=(this_7.h.push(new Text("repair_cost", String(item.repair_costs))),this_7);
    const b_1=(this_8.h.push(new Text("repair_status", item.repair_status)),this_8);
    const p_1=CompleteHoles(b_1.k, b_1.h, []);
    const i_1=new TemplateInstance(p_1[1], listitem(p_1[0]));
    let _2=(b_1.i=i_1,i_1);
    return _2.Doc;
  }, carData.v);
  const t=new ProviderBuilder("New_1");
  const this_1=(t.h.push(EventQ2(t.k, "onsend", () => t.i, () => {
    const _2=null;
    StartImmediate(Delay(() => {
      carData.Clear();
      return Bind(ReturnSessionId(), (a) => {
        sessionId().Set(a);
        return globalThis.sessionStorage.getItem("sessionId")==sessionId().Get()?(userEmail().Set(globalThis.sessionStorage.getItem("userEmail")),Bind(GetCarData(userEmail().Get(), userPermission.Get()), (a_1) => {
          serverRespons.Set(String(a_1));
          carData.AppendMany(a_1);
          return Bind(GetStatusNames(), (a_2) => {
            statusData.AppendMany(a_2);
            setTimeout(() => {
              const selects=globalThis.document.querySelectorAll(".repair_dropdown");
              for(let i_1=0, _3=selects.length-1;i_1<=_3;i_1++){
                const selectEl=selects[i_1];
                if(userPermission.Get()!="2")selectEl.disabled=true;
                else selectEl.disabled=false;
                const e=Get(statusData);
                try {
                  while(e.MoveNext())
                    {
                      const value=e.Current;
                      const opt=globalThis.document.createElement("option");
                      opt.value=value.status_name;
                      opt.text=value.status_name;
                      selectEl.append(opt);
                    }
                }
                finally {
                  if(typeof e=="object"&&isIDisposable(e))e.Dispose();
                }
              }
            }, 0);
            return Zero();
          });
        })):Zero();
      });
    }), null);
  })),t);
  const b=(this_1.h.push(new Elt("listcontainer", L)),this_1);
  const p=CompleteHoles(b.k, b.h, []);
  const i=new TemplateInstance(p[1], mainform_2(p[0]));
  let _1=(b.i=i,i);
  return _1.Doc;
}
export function RegisterCar(){
  const userPermission=Var.Create_1("");
  const failureData=Create((item) => item.failure_name, FSharpList.Empty);
  const serverRespons=Var.Create_1("");
  setTimeout(() => {
    userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
    if(userEmail().Get()!=""&&userEmail().Get()!=null){
      const menuEmail=globalThis.document.getElementById("LoginEmail");
      menuEmail.setAttribute("style", "visibility: visible");
      menuEmail.textContent=userEmail().Get();
    }
    const faulureSelect=globalThis.document.getElementById("failure");
    const _2=null;
    StartImmediate(Delay(() => Bind(GetFailureNames(), (a) => {
      failureData.AppendMany(a);
      return Combine(For(failureData, (a_1) => {
        const opt=globalThis.document.createElement("option");
        opt.value=String(a_1.failure_name);
        opt.text=String(a_1.failure_name);
        faulureSelect.appendChild(opt);
        return Zero();
      }), Delay(() => Bind(GetUserPermission(userEmail().Get()), (a_1) => {
        userPermission.Set(a_1);
        return userPermission.Get()=="2"?(globalThis.document.getElementById("StatusChange").setAttribute("style", "visibility: visible"),Zero()):Zero();
      })));
    })), null);
  }, 0);
  const t=new ProviderBuilder("New_1");
  const t_1=(t.h.push(EventQ2(t.k, "onsubmit", () => t.i, (e) => {
    const _2=null;
    StartImmediate(Delay(() => Bind(ReturnSessionId(), (a) => {
      let _3;
      userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
      if(userEmail().Get()!=""){
        const menuEmail=globalThis.document.getElementById("LoginEmail");
        _3=(menuEmail.setAttribute("style", "visibility: visible"),menuEmail.textContent=userEmail().Get(),Zero());
      }
      else _3=Zero();
      return Combine(_3, Delay(() => {
        sessionId().Set(a);
        return globalThis.sessionStorage.getItem("sessionId")==sessionId().Get()?Bind(CurrentUserId(password().Get(), userEmail().Get()), (a_1) => {
          const c=removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("licence")).Get());
          const m=removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("manuf")).Get());
          const newCar={
            car_licence:c, 
            user_id:BigInt(a_1), 
            c_type:removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("c_type")).Get()), 
            m_year:BigInt(Math.trunc(TemplateHole.Value(e.Vars.Hole("m_year")).Get())), 
            manuf:m, 
            failure:TemplateHole.Value(e.Vars.Hole("failure")).Get(), 
            repair_costs:TemplateHole.Value(e.Vars.Hole("repair_cost")).Get(), 
            repair_status:"1"
          };
          const isNonEmpty=(s) =>!IsNullOrWhiteSpace(s);
          let _4=isNonEmpty(newCar.car_licence)&&newCar.user_id>0n&&isNonEmpty(newCar.c_type)&&newCar.m_year>1900n&&isNonEmpty(newCar.manuf)&&isNonEmpty(newCar.failure)&&newCar.repair_costs>=0&&isNonEmpty(newCar.repair_status)?Bind(InsertCarData(newCar), (a_2) => {
            serverRespons.Set(a_2);
            return Zero();
          }):(serverRespons.Set("Please fill in all fields."),Zero());
          return Combine(_4, Delay(() => {
            alert(serverRespons.Get());
            globalThis.location.reload();
            return Zero();
          }));
        }):Zero();
      }));
    })), null);
  })),t);
  const b=(t_1.h.push(EventQ2(t_1.k, "onchange", () => t_1.i, (e) => {
    const failureName=TemplateHole.Value(e.Vars.Hole("failure")).Get();
    const failureCost=tryFind((item) => item.failure_name==failureName, failureData);
    const repairCost=failureCost==null?0:failureCost.$0.repair_costs;
    setTimeout(() => {
      globalThis.document.getElementById("repair_cost").value=String(repairCost);
    }, 0);
  })),t_1);
  const p=CompleteHoles(b.k, b.h, [["licence", 0, null], ["manuf", 0, null], ["c_type", 0, null], ["m_year", 1, null], ["failure", 0, null], ["repair_cost", 1, null]]);
  const i=new TemplateInstance(p[1], mainform_3(p[0]));
  let _1=(b.i=i,i);
  return _1.Doc;
}
export function SingingIn(){
  const userPermission=Var.Create_1("");
  setTimeout(() => {
    userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
    if(userEmail().Get()!=""&&userEmail().Get()!=null){
      const menuEmail=globalThis.document.getElementById("LoginEmail");
      menuEmail.setAttribute("style", "visibility: visible");
      menuEmail.textContent=userEmail().Get();
      const _2=null;
      StartImmediate(Delay(() => Bind(GetUserPermission(userEmail().Get()), (a) => {
        userPermission.Set(a);
        return userPermission.Get()=="2"?(globalThis.document.getElementById("StatusChange").setAttribute("style", "visibility: visible"),Zero()):Zero();
      })), null);
    }
  }, 0);
  const t=new ProviderBuilder("New_1");
  const b=(t.h.push(EventQ2(t.k, "onsubmit", () => t.i, (e) => {
    const _2=null;
    StartImmediate(Delay(() => {
      globalThis.sessionStorage.removeItem("userEmail");
      password().Set(removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("password")).Get()));
      userEmail().Set(removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("email")).Get()));
      return Bind(LogingInToDatabase(password().Get(), userEmail().Get()), (a) => {
        let _3;
        if(a==null)_3=(alert("No user found"),Zero());
        else {
          const sid=a.$0;
          _3=(sessionId().Set(sid),globalThis.sessionStorage.setItem("sessionId", sid),globalThis.sessionStorage.setItem("userEmail", userEmail().Get()),setTimeout(() => {
            userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
            const menuEmail=globalThis.document.getElementById("LoginEmail");
            menuEmail.setAttribute("style", "visibility: visible");
            menuEmail.textContent=userEmail().Get();
          }, 0),Bind(GetUserPermission(userEmail().Get()), (a_1) => {
            userPermission.Set(a_1);
            return Combine(userPermission.Get()=="2"?(globalThis.document.getElementById("StatusChange").setAttribute("style", "visibility: visible"),Zero()):Zero(), Delay(() => {
              alert("Welcome: "+String(userEmail().Get()));
              return Zero();
            }));
          }));
        }
        return Combine(_3, Delay(() => {
          TemplateHole.Value(e.Vars.Hole("password")).Set("");
          TemplateHole.Value(e.Vars.Hole("email")).Set("");
          return Zero();
        }));
      });
    }), null);
  })),t);
  const p=CompleteHoles(b.k, b.h, [["email", 0, null], ["password", 0, null]]);
  const i=new TemplateInstance(p[1], mainform_4(p[0]));
  let _1=(b.i=i,i);
  return _1.Doc;
}
export function UserRegistration(){
  const serverRespons=Var.Create_1("");
  const userPermission=Var.Create_1("");
  setTimeout(() => {
    userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
    if(userEmail().Get()!=""&&userEmail().Get()!=null){
      const menuEmail=globalThis.document.getElementById("LoginEmail");
      menuEmail.setAttribute("style", "visibility: visible");
      menuEmail.textContent=userEmail().Get();
      const _2=null;
      StartImmediate(Delay(() => Bind(GetUserPermission(userEmail().Get()), (a) => {
        userPermission.Set(a);
        return userPermission.Get()=="2"?(globalThis.document.getElementById("StatusChange").setAttribute("style", "visibility: visible"),Zero()):Zero();
      })), null);
    }
  }, 0);
  const t=new ProviderBuilder("New_1");
  const b=(t.h.push(EventQ2(t.k, "onsubmit", () => t.i, (e) => {
    const _2=null;
    StartImmediate(Delay(() => {
      const f=removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("firstname")).Get());
      const f_1=removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("lastname")).Get());
      const p_1=removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("password")).Get());
      const e_1=removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("email")).Get());
      const newUser={
        main_id:"", 
        family_name:f_1, 
        first_name:f, 
        password:p_1, 
        permission:BigInt(4), 
        phone_number:removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("phone")).Get()), 
        email:e_1, 
        city:removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("city")).Get()), 
        street:removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("street")).Get()), 
        house_number:removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("housenumber")).Get()), 
        floor_door:removeForbiddenCharacters(TemplateHole.Value(e.Vars.Hole("floordoor")).Get())
      };
      const isNonEmpty=(s) =>!IsNullOrWhiteSpace(s);
      let _3=isNonEmpty(newUser.family_name)&&isNonEmpty(newUser.first_name)&&isNonEmpty(newUser.password)&&isNonEmpty(newUser.phone_number)&&isNonEmpty(newUser.email)&&isNonEmpty(newUser.city)&&isNonEmpty(newUser.street)&&isNonEmpty(newUser.house_number)&&isNonEmpty(newUser.floor_door)?Bind(RegisterNewUser(newUser), (a) => {
        serverRespons.Set(a);
        return Zero();
      }):(serverRespons.Set("Please fill in all fields."),Zero());
      return Combine(_3, Delay(() => {
        alert(serverRespons.Get());
        globalThis.location.reload();
        return Zero();
      }));
    }), null);
  })),t);
  const p=CompleteHoles(b.k, b.h, [["firstname", 0, null], ["lastname", 0, null], ["email", 0, null], ["phone", 0, null], ["city", 0, null], ["street", 0, null], ["housenumber", 0, null], ["floordoor", 0, null], ["password", 0, null]]);
  const i=new TemplateInstance(p[1], mainform_5(p[0]));
  let _1=(b.i=i,i);
  return _1.Doc;
}
export function Main(){
  userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
  const userPermission=Var.Create_1("");
  if(userEmail().Get()!=""&&userEmail().Get()!=null){
    const menuEmail=globalThis.document.getElementById("LoginEmail");
    menuEmail.setAttribute("style", "visibility: visible");
    menuEmail.textContent=userEmail().Get();
    const _1=null;
    StartImmediate(Delay(() => Bind(GetUserPermission(userEmail().Get()), (a) => {
      userPermission.Set(a);
      return userPermission.Get()=="2"?(globalThis.document.getElementById("StatusChange").setAttribute("style", "visibility: visible"),Zero()):Zero();
    })), null);
  }
  const b=new ProviderBuilder("New_1");
  const p=CompleteHoles(b.k, b.h, []);
  const i=new TemplateInstance(p[1], mainform_6(p[0]));
  let _2=(b.i=i,i);
  return _2.Doc;
}
export function sessionId(){
  return $StartupCode_Client.sessionId;
}
export function password(){
  return $StartupCode_Client.password;
}
export function userEmail(){
  return $StartupCode_Client.userEmail;
}
