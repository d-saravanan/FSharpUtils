namespace Security.Services
open System.Security;
open System.Security.Cryptography;
open System.Xml;
open System;

type XmlDocumentProtector()=

    let BasicValidation source alg=
        if String.IsNullOrWhiteSpace source then DiagnosticUtility.RaiseException(DiagnosticUtility.Categories.XmlEncryption,nullArg("source"));
        if alg = null then DiagnosticUtility.RaiseException(DiagnosticUtility.Categories.XmlEncryption,nullArg("algorithm"));

    let ValidateDocAlgKeyName document algorithm keyname=
        if null = document then DiagnosticUtility.RaiseException(DiagnosticUtility.Categories.XmlEncryption,nullArg("document"));
        if algorithm = null then DiagnosticUtility.RaiseException(DiagnosticUtility.Categories.XmlEncryption,nullArg("algorithm"));
        if String.IsNullOrWhiteSpace keyname then DiagnosticUtility.RaiseException(DiagnosticUtility.Categories.XmlEncryption,nullArg("keyname"));

    let ValidateParameters document elementName elementIdToEncrypt algorithm keyName=
        ValidateDocAlgKeyName document  algorithm  keyName;
        if String.IsNullOrWhiteSpace elementName then DiagnosticUtility.RaiseException(DiagnosticUtility.Categories.XmlEncryption,nullArg("elementName"));
        if String.IsNullOrWhiteSpace elementIdToEncrypt then DiagnosticUtility.RaiseException(DiagnosticUtility.Categories.XmlEncryption,nullArg("elementIdToEncrypt"));
            
    let getDocumentFromSource content = 
        try
            let doc = new XmlDocument();
            doc.LoadXml content;
            doc;
        with
        | :? XmlException as ex -> DiagnosticUtility.Log(DiagnosticUtility.Categories.IdPMetadata,ex.Message); reraise();

    let Decrypt document algorithm keyName=
        ValidateDocAlgKeyName document  algorithm  keyName
        let exml = new System.Security.Cryptography.Xml.EncryptedXml(document);
        exml.AddKeyNameMapping(keyName ,algorithm);
        exml.DecryptDocument();

    member this.UnProtectWithAlgorithm(data2Decrypt:string, key:byte[] , algorithm:RSA )=
        BasicValidation data2Decrypt algorithm;

        let doc = getDocumentFromSource data2Decrypt;

        try
            Decrypt doc algorithm "rsaKey";
        finally
            algorithm.Clear();
        ();
    member this.ProtectWithAlgorithm(source:string , key:byte[] , algorithm:RSA)=
        BasicValidation source algorithm;
        let xmlDoc = getDocumentFromSource source;
        try
            Eecrypt doc algorithm "rsaKey";
        finally
            algorithm.Clear();
        ();

    member this.Encrypt(document:XmlDocument,elementName:string,elementIdToEncrypt:string, algorithm:RSA, keyName:string)=
        ValidateParameters document elementName elementIdToEncrypt algorithm keyName;
        let elementToEncrypt = document.GetElementsByTagName(elementName).[0];
        if (elementToEncrypt = null) then DiagnosticUtility.RaiseException(DiagnosticUtility.Categories.XmlEncryption,new XmlException("invalid xml"));
        let sessionKey = new RijndaelManaged();
        try
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Create a new instance of the EncryptedXml class and use it to encrypt the XmlElement with the a new random symmetric key.
            // Create a 256 bit Rijndael key.
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            sessionKey.KeySize = 256;

            let eXml = new EncryptedXml();

            let encryptedElement = eXml.EncryptData(elementToEncrypt, sessionKey, false);
            ////////////////////////////////////////////////////////////////////////////////////////////////
            // Construct an EncryptedData object and populate it with the desired encryption information.
            ////////////////////////////////////////////////////////////////////////////////////////////////

            let edElement = new EncryptedData();
            edElement.Type = EncryptedXml.XmlEncElementUrl;
            edElement.Id = elementIdToEncrypt;
            // Create an EncryptionMethod element so that the receiver knows which algorithm to use for decryption.

            edElement.EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncAES256Url);
            // Encrypt the session key and add it to an EncryptedKey element.
            let ek = new EncryptedKey();

            let encryptedKey = EncryptedXml.EncryptKey(sessionKey.Key, algorithm, false);

            ek.CipherData = new CipherData(encryptedKey);

            ek.EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncRSA15Url);

            // Create a new DataReference element for the KeyInfo element.  This optional element specifies which EncryptedData
            // uses this key.  An XML document can have multiple EncryptedData elements that use different keys.
            let dRef = new DataReference();
            dref.Uri = "#"+elementIdToEncrypt;

            // Add the DataReference to the EncryptedKey.
            ek.AddReference(dRef);
            // Add the encrypted key to the
            // EncryptedData object.

            edElement.KeyInfo.AddClause(new KeyInfoEncryptedKey(ek));
            // Set the KeyInfo element to specify the
            // name of the RSA key.

            // Create a new KeyInfoName element.
            let kin = new KeyInfoName { Value = keyName };

            // Add the KeyInfoName element to the
            // EncryptedKey object.
            ek.KeyInfo.AddClause(kin);
            // Add the encrypted element data to the
            // EncryptedData object.
            edElement.CipherData.CipherValue = encryptedElement;
            ////////////////////////////////////////////////////
            // Replace the element from the original XmlDocument
            // object with the EncryptedData element.
            ////////////////////////////////////////////////////
            EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
        finally
           if (!sessionKey = null) then sessionKey.Clear();
        ();