import { Create } from "../WebSharper.UI/WebSharper.UI.ListModel.js"
import FSharpList from "../WebSharper.StdLib/Microsoft.FSharp.Collections.FSharpList`1.js"
import Var from "../WebSharper.UI/WebSharper.UI.Var.js"
import { StartImmediate, Delay, Bind, Zero, For, Combine } from "../WebSharper.StdLib/WebSharper.Concurrency.js"
import { GetUserPermission, GetCarData, GetStatusNames, GetFailureNames, CurrentUserId, InsertCarData, LogingInToDatabase, RegisterNewUser } from "./AlphaProject.Server.js"
import Doc from "../WebSharper.UI/WebSharper.UI.Doc.js"
import ProviderBuilder from "../WebSharper.UI.Templating.Runtime/WebSharper.UI.Templating.Runtime.Server.ProviderBuilder.js"
import Text from "../WebSharper.UI/WebSharper.UI.TemplateHoleModule.Text.js"
import { CompleteHoles, EventQ2 } from "../WebSharper.UI.Templating.Runtime/WebSharper.UI.Templating.Runtime.Server.Handler.js"
import TemplateInstance from "../WebSharper.UI.Templating.Runtime/WebSharper.UI.Templating.Runtime.Server.TemplateInstance.js"
import { listitem, mainform, mainform_1, mainform_2, mainform_3, mainform_4 } from "./$Generated.js"
import { Get } from "../WebSharper.StdLib/WebSharper.Enumerator.js"
import { isIDisposable } from "../WebSharper.StdLib/System.IDisposable.js"
import Elt from "../WebSharper.UI/WebSharper.UI.TemplateHoleModule.Elt.js"
import TemplateHole from "../WebSharper.UI/WebSharper.UI.TemplateHole.js"
import { IsNullOrWhiteSpace } from "../WebSharper.StdLib/Microsoft.FSharp.Core.StringModule.js"
import { tryFind } from "../WebSharper.StdLib/Microsoft.FSharp.Collections.SeqModule.js"
import { iter, ofArray } from "../WebSharper.StdLib/Microsoft.FSharp.Collections.ListModule.js"
import $StartupCode_Client from "./$StartupCode_Client.js"
export function CarStatus(){
  const carData=Create((item) => item.car_licence, FSharpList.Empty);
  const statusData=Create((item) => item.status_name, FSharpList.Empty);
  const serverRespons=Var.Create_1("");
  const userPermission=Var.Create_1("");
  setTimeout(() => {
    userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
    globalThis.document.getElementById("LoginEmail").textContent=userEmail().Get();
    if(userEmail().Get()==""||userEmail().Get()==null)userPermission.Set("4");
    else {
      const _2=null;
      StartImmediate(Delay(() => Bind(GetUserPermission(userEmail().Get()), (a) => {
        userPermission.Set(a);
        return Zero();
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
      userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
      return Bind(GetCarData(userEmail().Get(), userPermission.Get()), (a) => {
        serverRespons.Set(String(a));
        carData.AppendMany(a);
        return Bind(GetStatusNames(), (a_1) => {
          statusData.AppendMany(a_1);
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
      });
    }), null);
  })),t);
  const b=(this_1.h.push(new Elt("listcontainer", L)),this_1);
  const p=CompleteHoles(b.k, b.h, []);
  const i=new TemplateInstance(p[1], mainform(p[0]));
  let _1=(b.i=i,i);
  return _1.Doc;
}
export function RegisterCar(){
  const failureData=Create((item) => item.failure_name, FSharpList.Empty);
  const serverRespons=Var.Create_1("");
  setTimeout(() => {
    userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
    globalThis.document.getElementById("LoginEmail").textContent=userEmail().Get();
    const selectEl=globalThis.document.getElementById("failure");
    const _2=null;
    StartImmediate(Delay(() => Bind(GetFailureNames(), (a) => {
      failureData.AppendMany(a);
      return For(failureData, (a_1) => {
        const opt=globalThis.document.createElement("option");
        opt.value=String(a_1.failure_name);
        opt.text=String(a_1.failure_name);
        selectEl.appendChild(opt);
        return Zero();
      });
    })), null);
  }, 0);
  const t=new ProviderBuilder("New_1");
  const t_1=(t.h.push(EventQ2(t.k, "onsubmit", () => t.i, (e) => {
    const _2=null;
    StartImmediate(Delay(() => {
      userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
      globalThis.document.getElementById("LoginEmail").textContent=userEmail().Get();
      return userEmail().Get()!=""&&userEmail().Get()!=null?Bind(CurrentUserId(password().Get(), userEmail().Get()), (a) => {
        const c=TemplateHole.Value(e.Vars.Hole("licence")).Get();
        const m=TemplateHole.Value(e.Vars.Hole("manuf")).Get();
        const newCar={
          car_licence:c, 
          user_id:BigInt(a), 
          c_type:TemplateHole.Value(e.Vars.Hole("c_type")).Get(), 
          m_year:BigInt(Math.trunc(TemplateHole.Value(e.Vars.Hole("m_year")).Get())), 
          manuf:m, 
          failure:TemplateHole.Value(e.Vars.Hole("failure")).Get(), 
          repair_costs:TemplateHole.Value(e.Vars.Hole("repair_cost")).Get(), 
          repair_status:"1"
        };
        const isNonEmpty=(s) =>!IsNullOrWhiteSpace(s);
        let _3=isNonEmpty(newCar.car_licence)&&newCar.user_id>0n&&isNonEmpty(newCar.c_type)&&newCar.m_year>1900n&&isNonEmpty(newCar.manuf)&&isNonEmpty(newCar.failure)&&newCar.repair_costs>=0&&isNonEmpty(newCar.repair_status)?Bind(InsertCarData(newCar), (a_1) => {
          serverRespons.Set(a_1);
          return serverRespons.Get()=="Your Registration is comlete!"?(TemplateHole.Value(e.Vars.Hole("licence")).Set(""),TemplateHole.Value(e.Vars.Hole("manuf")).Set(""),TemplateHole.Value(e.Vars.Hole("c_type")).Set(""),TemplateHole.Value(e.Vars.Hole("m_year")).Set(0),TemplateHole.Value(e.Vars.Hole("failure")).Set(""),TemplateHole.Value(e.Vars.Hole("repair_cost")).Set(0),Zero()):Zero();
        }):(serverRespons.Set("Please fill in all fields."),Zero());
        return Combine(_3, Delay(() => {
          alert(serverRespons.Get());
          return Zero();
        }));
      }):Zero();
    }), null);
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
  const i=new TemplateInstance(p[1], mainform_1(p[0]));
  let _1=(b.i=i,i);
  return _1.Doc;
}
export function SingingIn(){
  const serverRespons=Var.Create_1("");
  setTimeout(() => {
    userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
    globalThis.document.getElementById("LoginEmail").textContent=userEmail().Get();
  }, 0);
  const t=new ProviderBuilder("New_1");
  const b=(t.h.push(EventQ2(t.k, "onsubmit", () => t.i, (e) => {
    const _2=null;
    StartImmediate(Delay(() => {
      globalThis.sessionStorage.removeItem("userEmail");
      password().Set(TemplateHole.Value(e.Vars.Hole("password")).Get());
      userEmail().Set(TemplateHole.Value(e.Vars.Hole("email")).Get());
      return Bind(LogingInToDatabase(password().Get(), userEmail().Get()), (a) => {
        serverRespons.Set(a);
        return Combine(a=="No user found"?(alert("No user found"),Zero()):(globalThis.sessionStorage.setItem("userEmail", userEmail().Get()),alert("Welcome "+String(serverRespons.Get())),Zero()), Delay(() => {
          TemplateHole.Value(e.Vars.Hole("password")).Set("");
          TemplateHole.Value(e.Vars.Hole("email")).Set("");
          setTimeout(() => {
            userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
            globalThis.document.getElementById("LoginEmail").textContent=userEmail().Get();
          }, 0);
          return Zero();
        }));
      });
    }), null);
  })),t);
  const p=CompleteHoles(b.k, b.h, [["email", 0, null], ["password", 0, null]]);
  const i=new TemplateInstance(p[1], mainform_2(p[0]));
  let _1=(b.i=i,i);
  return _1.Doc;
}
export function UserRegistration(){
  const serverRespons=Var.Create_1("");
  setTimeout(() => {
    userEmail().Set(globalThis.sessionStorage.getItem("userEmail"));
    globalThis.document.getElementById("LoginEmail").textContent=userEmail().Get();
  }, 0);
  const t=new ProviderBuilder("New_1");
  const b=(t.h.push(EventQ2(t.k, "onsubmit", () => t.i, (e) => {
    const _2=null;
    StartImmediate(Delay(() => {
      const f=TemplateHole.Value(e.Vars.Hole("firstname")).Get();
      const f_1=TemplateHole.Value(e.Vars.Hole("lastname")).Get();
      const p_1=TemplateHole.Value(e.Vars.Hole("password")).Get();
      const e_1=TemplateHole.Value(e.Vars.Hole("email")).Get();
      const newUser={
        main_id:"", 
        family_name:f_1, 
        first_name:f, 
        password:p_1, 
        permission:BigInt(4), 
        phone_number:TemplateHole.Value(e.Vars.Hole("phone")).Get(), 
        email:e_1, 
        city:TemplateHole.Value(e.Vars.Hole("city")).Get(), 
        street:TemplateHole.Value(e.Vars.Hole("street")).Get(), 
        house_number:TemplateHole.Value(e.Vars.Hole("housenumber")).Get(), 
        floor_door:TemplateHole.Value(e.Vars.Hole("floordoor")).Get()
      };
      const isNonEmpty=(s) =>!IsNullOrWhiteSpace(s);
      let _3=isNonEmpty(newUser.family_name)&&isNonEmpty(newUser.first_name)&&isNonEmpty(newUser.password)&&isNonEmpty(newUser.phone_number)&&isNonEmpty(newUser.email)&&isNonEmpty(newUser.city)&&isNonEmpty(newUser.street)&&isNonEmpty(newUser.house_number)&&isNonEmpty(newUser.floor_door)?Bind(RegisterNewUser(newUser), (a) => {
        serverRespons.Set(a);
        if(serverRespons.Get()=="Your registration is complete!"){
          const e_2=e.Vars;
          iter((v) => {
            v.Set("");
          }, ofArray([TemplateHole.Value(e_2.Hole("firstname")), TemplateHole.Value(e_2.Hole("lastname")), TemplateHole.Value(e_2.Hole("email")), TemplateHole.Value(e_2.Hole("phone")), TemplateHole.Value(e_2.Hole("city")), TemplateHole.Value(e_2.Hole("street")), TemplateHole.Value(e_2.Hole("housenumber")), TemplateHole.Value(e_2.Hole("floordoor")), TemplateHole.Value(e_2.Hole("password"))]));
          return Zero();
        }
        else return Zero();
      }):(serverRespons.Set("Please fill in all fields."),Zero());
      return Combine(_3, Delay(() => {
        alert(serverRespons.Get());
        return Zero();
      }));
    }), null);
  })),t);
  const p=CompleteHoles(b.k, b.h, [["firstname", 0, null], ["lastname", 0, null], ["email", 0, null], ["phone", 0, null], ["city", 0, null], ["street", 0, null], ["housenumber", 0, null], ["floordoor", 0, null], ["password", 0, null]]);
  const i=new TemplateInstance(p[1], mainform_3(p[0]));
  let _1=(b.i=i,i);
  return _1.Doc;
}
export function Main(){
  const b=new ProviderBuilder("New_1");
  const p=CompleteHoles(b.k, b.h, []);
  const i=new TemplateInstance(p[1], mainform_4(p[0]));
  let _1=(b.i=i,i);
  return _1.Doc;
}
export function password(){
  return $StartupCode_Client.password;
}
export function userEmail(){
  return $StartupCode_Client.userEmail;
}
