export function isIComparable(x):x is IComparable<T0>
export default interface IComparable<T0>{
  CompareTo(a:T0):number
}
