namespace Helpers
open System.Text;
open System;
open Utils;

module EncodingHelpers=

    let Base64Encode (input:string, codePage:int) =
        Encoding.GetEncoding(codePage).GetBytes(input) |> Convert.ToBase64String;

    let Base64Decode (input:string, codePage:int) =
        Convert.FromBase64String(input) |> Encoding.GetEncoding(codePage).GetString;