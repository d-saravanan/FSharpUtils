module MetadataParser
open System.Configuration;
open System;
open System.Xml;
open System.IO;
open System.IdentityModel.Metadata;
open System.Security.Cryptography.X509Certificates;
open System.ServiceModel.Security;
open System.IdentityModel.Selectors

let SkipMetadataCertificateValidation:bool=
    match ConfigurationManager.AppSettings.["SkipMetadataCertificateValidation"] with
    | "" -> false
    | "true" -> true
    | _ -> false;

let CertificateValidationCallback sender certificate chain sslPolicyErrors= true;
let private cvc = new Net.Security.RemoteCertificateValidationCallback(CertificateValidationCallback);

let DeconstructFromContent metadata =
    if String.IsNullOrWhiteSpace metadata then failwith "empty or invalid metadata";
    use strReader = new StringReader(metadata);
    let xmlreader = new XmlTextReader(strReader);
    let serializer = new MetadataSerializer();
    serializer.CertificateValidationMode = X509CertificateValidationMode.None |> ignore;
    if(SkipMetadataCertificateValidation) then do
        System.Net.ServicePointManager.ServerCertificateValidationCallback =cvc |> ignore;
        serializer.CertificateValidator <- X509CertificateValidator.None;
    
    let descriptorMetadata = serializer.ReadMetadata(xmlreader);
    downcast descriptorMetadata :> EntityDescriptor;