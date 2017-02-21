module DiagnosticUtility
    open System;
    open System.Configuration;
    open System.Threading;
    open System.Threading.Tasks;

    let enableDiagnosticsInRelease:bool=
        match ConfigurationManager.AppSettings.["EnableDiagnostics"] with
        | "" -> false
        | "true" -> true
        | _ -> false;

    let formattedMessage input args=
        System.String.Format(System.Globalization.CultureInfo.InvariantCulture,input,args);
    
    let LogWithFormat(category,message,[<ParamArray>]args) =
    #if DEBUG
                 System.Diagnostics.Trace.WriteLine((formattedMessage message args),category);
    #else
                 if enableDiagnosticsInRelease then
                     System.Diagnostics.Trace.WriteLine((formattedMessage message args),category);
    #endif

    let Log (category,message) =
    #if DEBUG
                 System.Diagnostics.Trace.WriteLine(message,category);
    #else
                 if enableDiagnosticsInRelease then
                     System.Diagnostics.Trace.WriteLine(message,category);
    #endif
    
    let RaiseException<'E when 'E :>Exception>(category:string,ex: 'E)=
        Log (category,ex.ToString());
        raise (ex);
        ();


    module Categories=
        let Saml2Request="Saml2Request";
        let Saml2Response="Saml2Response";
        let MTArgs = "MTArgs";
        let MTValidation = "MTValidation";
        let IdPMetadata = "IdPMetadata";
        let TenantIdPMetadata = "TenantIdPMetadata";
        let MetadataEncryption = "MetadataEncryption";
        let XmlEncryption="XmlEncryption";
    
