﻿#pragma checksum "..\..\..\TicketBoxManager\TickBoxPutIn.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1FBFF633EA83437A2D5293D9CBB6C71D92F4EA23"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using AFC.BOM2.UIController;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Components;
using AFC.WS.UI.UIPage.TicketBoxManager;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;
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


namespace AFC.WS.UI.UIPage.TicketBoxManager {
    
    
    /// <summary>
    /// TickBoxPutIn
    /// </summary>
    public partial class TickBoxPutIn : AFC.BOM2.UIController.UserControlBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border DiagramBorder;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid rootLayout;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AFC.WS.UI.UIPage.TicketBoxManager.TicketBoxRfidInfo rfidInfo;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtRFIDNum;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbType;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPutNo;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtTotal;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnReadRFID;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPutIn;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRfidConnect;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnReset;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Windows.Controls.DataGrid dgTicketBoxInInfo;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AFC.WS.UI.UIPage.TicketBoxManager.TickBoxClear tickBoxClear;
        
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
            System.Uri resourceLocater = new System.Uri("/AFC.WS.UI.UIPage;component/ticketboxmanager/tickboxputin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.DiagramBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            
            #line 13 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
            ((System.Windows.Controls.TabControl)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.TabControl_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.rootLayout = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.rfidInfo = ((AFC.WS.UI.UIPage.TicketBoxManager.TicketBoxRfidInfo)(target));
            return;
            case 5:
            this.txtRFIDNum = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.cmbType = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.txtPutNo = ((System.Windows.Controls.TextBox)(target));
            
            #line 47 "..\..\..\TicketBoxManager\TickBoxPutIn.xaml"
            this.txtPutNo.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtPutNo_TextChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.txtTotal = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.btnReadRFID = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.btnPutIn = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.btnRfidConnect = ((System.Windows.Controls.Button)(target));
            return;
            case 12:
            this.btnReset = ((System.Windows.Controls.Button)(target));
            return;
            case 13:
            this.dgTicketBoxInInfo = ((Microsoft.Windows.Controls.DataGrid)(target));
            return;
            case 14:
            this.tickBoxClear = ((AFC.WS.UI.UIPage.TicketBoxManager.TickBoxClear)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

