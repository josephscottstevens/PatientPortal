 type Name =
   | FirstName of string
   | Id of int

let x = FirstName "John"
let y = Id 5

let z =
  match y with
  | FirstName t -> t
  | Id t -> string t