import { length } from "./Microsoft.FSharp.Core.LanguagePrimitives.IntrinsicFunctions.js"
import { findIndexBound } from "./System.Array.js"
import { Equals } from "./Microsoft.FSharp.Core.Operators.Unchecked.js"
export function RemoveAll(this_1, pred){
  let removed=0;
  let i=0;
  while(i<length(this_1))
    if(pred(this_1[i])){
      let j=i+1;
      while(j<length(this_1)&&pred(this_1[j]))
        j=j+1;
      removed=removed+j-i;
      let delete_1=j-i;
      this_1.splice(i, delete_1);
    }
    else i=i+1;
  return removed;
}
export function Remove(this_1, item){
  const m=findIndexBound(this_1, 0, length(this_1), item==null?(v) => v==null:(o) => {
    let c;
    c=item;
    return Equals(c, o);
  });
  return m===-1?false:(this_1.splice(m, 1),true);
}
