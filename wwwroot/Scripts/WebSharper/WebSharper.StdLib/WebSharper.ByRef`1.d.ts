export function isByRef(x):x is ByRef<T0>
export default interface ByRef<T0>{
  get():T0
  set(a:T0):void
}
