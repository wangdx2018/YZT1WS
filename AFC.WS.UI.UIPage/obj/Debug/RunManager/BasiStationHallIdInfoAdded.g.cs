﻿#pragma checksum "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B962FFC4ACB99B7B2B12CB20378014C8C55C5B01"
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


namespace AFC.WS.UI.UIPage.RunManager {
    
    
    /// <summary>
    /// BasiStationHallIdInfoAdded
    /// </summary>
    public partial class BasiStationHallIdInfoAdded : AFC.BOM2.UIController.UserControlBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border DiagramBorder;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid rootLayout;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox LineName;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox StationName;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AFC.WS.UI.CommonControls.TextBoxExtend StationHallId;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AFC.WS.UI.CommonControls.TextBoxExtend StationHallName;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button1;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button2;
        
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
            System.Uri resourceLocater = new System.Uri("/AFC.WS.UI.UIPage;component/runmanager/basistationhallidinfoadded.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml"
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
            this.DiagramBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.rootLayout = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.LineName = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.StationName = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.StationHallId = ((AFC.WS.UI.CommonControls.TextBoxExtend)(target));
            return;
            case 6:
            this.StationHallName = ((AFC.WS.UI.CommonControls.TextBoxExtend)(target));
            return;
            case 7:
            this.button1 = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml"
            this.button1.Click += new System.Windows.RoutedEventHandler(this.btnAddProvider_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.button2 = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\..\RunManager\BasiStationHallIdInfoAdded.xaml"
            this.button2.Click += new System.Windows.RoutedEventHandler(this.btnReset_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

