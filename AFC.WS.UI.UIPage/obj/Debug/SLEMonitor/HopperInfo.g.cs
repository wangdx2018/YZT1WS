﻿#pragma checksum "..\..\..\SLEMonitor\HopperInfo.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1DCB83828DCDACBBB313E529E46AD75E4F3CC186"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using AFC.WS.UI.CommonControls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AFC.WS.UI.UIPage.SLEMonitor {
    
    
    /// <summary>
    /// HopperInfo
    /// </summary>
    public partial class HopperInfo : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 7 "..\..\..\SLEMonitor\HopperInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scroll21;
        
        #line default
        #line hidden
        
        
        #line 8 "..\..\..\SLEMonitor\HopperInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel stackPanelTicketInfo;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\..\SLEMonitor\HopperInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox header;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\SLEMonitor\HopperInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AFC.WS.UI.CommonControls.TextBoxExtend txtTicketNumStatus;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\SLEMonitor\HopperInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AFC.WS.UI.CommonControls.TextBoxExtend txtTicketCurrentNum;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AFC.WS.UI.UIPage;component/slemonitor/hopperinfo.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SLEMonitor\HopperInfo.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.scroll21 = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 2:
            this.stackPanelTicketInfo = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.header = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 4:
            this.txtTicketNumStatus = ((AFC.WS.UI.CommonControls.TextBoxExtend)(target));
            return;
            case 5:
            this.txtTicketCurrentNum = ((AFC.WS.UI.CommonControls.TextBoxExtend)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

