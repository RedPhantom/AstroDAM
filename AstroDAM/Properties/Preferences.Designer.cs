﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AstroDAM.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.3.0.0")]
    internal sealed partial class Preferences : global::System.Configuration.ApplicationSettingsBase {
        
        private static Preferences defaultInstance = ((Preferences)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Preferences())));
        
        public static Preferences Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{dt|hh:mm:ss} - {on}")]
        public string TreeNodeFormat {
            get {
                return ((string)(this["TreeNodeFormat"]));
            }
            set {
                this["TreeNodeFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PlaySplashClip {
            get {
                return ((bool)(this["PlaySplashClip"]));
            }
            set {
                this["PlaySplashClip"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int NodeGrouping {
            get {
                return ((int)(this["NodeGrouping"]));
            }
            set {
                this["NodeGrouping"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool TreeIsAscending {
            get {
                return ((bool)(this["TreeIsAscending"]));
            }
            set {
                this["TreeIsAscending"] = value;
            }
        }
    }
}
