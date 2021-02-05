# Edsger

Edsger is a program synthesis system for deriving programs from formal specifications. 
It uses the [Boogie](https://github.com/boogie-org/boogie) verification language to specify programs and their specifications.  

Edsger uses the Boogie verifier to generate the verification conditions and 
the Sygus solvers to synthesize the unknown program fragments.  


## Dependencies
Edsger requires [.NET Core](https://dotnet.microsoft.com/) and the [CVC4](https://cvc4.github.io/)(v1.8) solver.

Edsger also depends on Boogie. No separate installation of Boogie is required 
as a modified version is built along with Edsger (see below).

## Building

To build Edsger (and Boogie) run:

```
mkdir BASE-DIRECTORY
cd BASE-DIRECTORY
```

```
cd BASE-DIRECTORY
git clone <repository-url> --recurse-submodules
dotnet build Source/Edsger.sln
```

> **WARNING:** There is currently a build problem with Boogie with .NET Core and GitVersionTask.
The workaround is to set the environment variable `MSBUILDSINGLELOADCONTEXT=1` and run `dotnet build-server shutdown`


Location of the compiled Edsger binary:

`Source/EdsgerDriver/bin/${CONFIGURATION}/${FRAMEWORK}/EdsgerDriver.dll`

## Backend SMT Solver

For synthesis, Edsger currently supports [CVC4](https://cvc4.github.io/) solver. 

<!-- Call Edsger with `/proverOpt:SOLVER=CVC4`. -->

Edsger looks for an executable named `cvc4` in `PATH`. 
<!-- You can explicitly specify the
solver name as `/proverOpt:PROVER_NAME=<exeName>` and solver path as `/ProverOpt:PROVER_PATH=<path>`. -->


## License

Edsger and our modifications to boogie are licensed under the Apache License (Version 2). See [LICENSE.md](LICENSE.md) for details.
The thirdparty dependencies (e.g. the original Boogie source) have their respective licenses.

