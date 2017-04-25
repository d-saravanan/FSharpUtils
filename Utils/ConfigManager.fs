namespace ConfigurationManagement

open System
open System.Configuration;
open System.Collections.Concurrent;

type IAppConfigManager=
    abstract member Get: key:string -> string;
    abstract member ValidatedGet: key:string -> string;
    abstract member Flush: unit -> unit;

type AppSettingsConfigManager()=
    static let _store = new ConcurrentDictionary<string,string>();
    interface IAppConfigManager with 
        member this.Get key=
            _store.GetOrAdd(key,ConfigurationManager.AppSettings.[key]);
        member this.ValidatedGet key=
            let result = _store.GetOrAdd(key,ConfigurationManager.AppSettings.[key]);
            if String.IsNullOrWhiteSpace result then failwith ("The key "+key+" does not exist in the configuration source");
            else result;
        member this.Flush() =
            _store.Clear();