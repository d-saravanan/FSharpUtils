﻿module DiagnosticUtility

open System
open System.Configuration

    //let enableDiagnosticsInRelease:bool=
    //    match ConfigurationManager.AppSettings.["EnableDiagnostics"] with
    //    | "" -> false
    //    | "true" -> true
    //    | _ -> false;

    //let formattedMessage input args=
    //    System.String.Format(System.Globalization.CultureInfo.InvariantCulture,input,args);
    
    //let LogWithFormat(category,message,[<ParamArray>]args) =
    //#if DEBUG
    //             System.Diagnostics.Trace.WriteLine((formattedMessage message args),category);
    //#else
    //             if enableDiagnosticsInRelease then
    //                 System.Diagnostics.Trace.WriteLine((formattedMessage message args),category);
    //#endif

    //let Log (category,message) =
    //#if DEBUG
    //             System.Diagnostics.Trace.WriteLine(message,category);
    //#else
    //             if enableDiagnosticsInRelease then
    //                 System.Diagnostics.Trace.WriteLine(message,category);
    //#endif
    
    let RaiseException<'E when 'E :>Exception>(category:string,ex: 'E)=
    
        raise (ex);
        ();


    module Categories=
        let Saml2Signout="Saml2Signout";
        let Saml2Request="Saml2Request";
        let Saml2Response="Saml2Response";
        let MTArgs = "MTArgs";
        let MTValidation = "MTValidation";
        let IdPMetadata = "IdPMetadata";
        let TenantIdPMetadata = "TenantIdPMetadata";
        let MetadataEncryption = "MetadataEncryption";
        let XmlEncryption="XmlEncryption";
        let JsonData="JsonData";
        let DataValidation = "DataValidation";
        let PerfTimer= "PerfTimer";
    
