export function isIDelegateEvent(x):x is IDelegateEvent<T0>
export default interface IDelegateEvent<T0>{
  AddHandler(a:T0):void
  RemoveHandler(a:T0):void
}
