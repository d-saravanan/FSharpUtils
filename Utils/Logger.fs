namespace Logging
open System;
open System.Threading.Tasks
open System.Configuration

type ILogger =
    abstract member Log:  category:string -> message:string ->bool;
    abstract member LogWithFormat: category:string ->message:string -> args:Object[] -> bool;


type DefaultLogger()=
    let enableDiagnosticsInRelease:bool=
        match ConfigurationManager.AppSettings.["EnableDiagnostics"] with
        | "" -> false
        | "true" -> true
        | _ -> false;

    let formattedMessage input args=
        System.String.Format(Globalization.CultureInfo.InvariantCulture,input,args);

    interface ILogger with
        member this.Log category message=
            System.Diagnostics.Trace.WriteLine(message,category);
            true;
        member this.LogWithFormat category message args=
        #if DEBUG
            System.Diagnostics.Trace.WriteLine((formattedMessage message args),category);
        #else
            if enableDiagnosticsInRelease then
                System.Diagnostics.Trace.WriteLine((formattedMessage message args),category);
        #endif
            true;