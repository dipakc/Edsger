#!/usr/bin/env bash

# Adapted from the Dafny script.
# find the source directory for this script even if it's been symlinked [issue #185]
# from https://stackoverflow.com/questions/59895/
SOURCE="${BASH_SOURCE[0]}"
while [ -h "$SOURCE" ]; do
    DIR="$( cd -P "$( dirname "$SOURCE" )" && pwd )"
    SOURCE="$(readlink "$SOURCE")"
    [[ $SOURCE != /* ]] && SOURCE="$DIR/$SOURCE"
done
EDSGER_ROOT="$( cd -P "$( dirname "$SOURCE" )" && pwd )"

MY_OS=$(uname -s)
if [ "${MY_OS:0:5}" == "MINGW" ] || [ "${MY_OS:0:6}" == "CYGWIN" ]; then
    EDSGER_EXE_NAME="EdsgerDriver.exe"
else
    EDSGER_EXE_NAME="EdsgerDriver.dll"
fi
EDSGER="$EDSGER_ROOT/$EDSGER_EXE_NAME"
if [[ ! -e "$EDSGER" ]]; then
    echo "Error: $EDSGER_EXE_NAME not found at $EDSGER_ROOT."
    exit 1
fi

if [ "${MY_OS:0:5}" == "MINGW" ] || [ "${MY_OS:0:6}" == "CYGWIN" ]; then
    "$EDSGER" "$@"
else
    DOTNET=$(type -p dotnet)
    if [[ ! -x "$DOTNET" ]]; then
        echo "Error: Edsger requires .NET Core to run on non-Windows systems."
        exit 1
    fi
    "$DOTNET" "$EDSGER" "$@"
fi
