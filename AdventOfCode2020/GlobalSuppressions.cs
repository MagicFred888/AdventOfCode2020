// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Blocker Code Smell", "S2368:Public methods should not have multidimensional array parameters", Justification = "This is a debug methode to display an array", Scope = "member", Target = "~M:AdventOfCode2020.Tools.SmallTools.DebugPrint(System.Object[,],System.Collections.Generic.Dictionary{System.String,System.String},System.String)")]
[assembly: SuppressMessage("Blocker Code Smell", "S2368:Public methods should not have multidimensional array parameters", Justification = "This is a debug methode to display an array", Scope = "member", Target = "~M:AdventOfCode2020.Tools.SmallTools.DebugPrint(System.Char[,])")]
[assembly: SuppressMessage("Major Code Smell", "S4200:Native methods should be wrapped", Justification = "<Pending>", Scope = "member", Target = "~M:AdventOfCode2020.Program.IsKeyPressed(System.Int32)~System.Boolean")]
[assembly: SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time", Justification = "<Pending>", Scope = "member", Target = "~M:AdventOfCode2020.Program.GetAsyncKeyState(System.Int32)~System.Int16")]
[assembly: SuppressMessage("Blocker Code Smell", "S2437:Unnecessary bit operations should not be performed", Justification = "It's necessary", Scope = "member", Target = "~M:AdventOfCode2020.Solver.Day14.GetSolution1(System.Boolean)~System.String")]