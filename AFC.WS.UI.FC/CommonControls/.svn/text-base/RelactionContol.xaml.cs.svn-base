using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// RelactionContol.xaml 的交互逻辑
    /// </summary>
    public partial class RelactionContol : UserControl
    {
        public RelactionContol()
        {
            InitializeComponent();
           // Binding();
        }

        public ObservableCollection<Data> left
        {
            set;
            get;
        }

        public ObservableCollection<Data> current
        {
            set;
            get;
        }

        /// <summary>
        /// 设置CheckBox的名称
        /// </summary>
        /// <param name="content"></param>
        public void SetCheckBoxContent(string content)
        {
            this.cbContent.Content = content;
        }

        /// <summary>
        /// 设置是否为可见
        /// </summary>
        /// <param name="visible"></param>
        public void SetContentVisable(Visibility visible)
        {
            /*if (Visibility.Visible == visible)
            {
                this.cbContent.IsChecked = true;
            }
            else
            {
                this.cbContent.IsChecked = false;
            }*/
            this.cbContent.Visibility = visible;
        }

        /// <summary>
        /// 返回是否选中
        /// </summary>
        public bool GetCheckBoxIsChecked
        {
            get
            {
                return this.cbContent.IsChecked.Value;
            }
        }

        public void BindingCurrent(ObservableCollection<Data> list)
        {
            this.current = list;
            this.listCurrent.ItemsSource = list;
        }

        public void BindingLeft(ObservableCollection<Data> list)
        {
            this.left = list;
            this.listLeft.ItemsSource = list;
        }

        public void SetGroupHeader(string header)
        {
            this.groupControl.Header = header;
        }

        public void SetCurrentLabel(string label)
        {
            this.label1.Content = label;
        }

        public void SetLeftLabel(string label)
        {
            this.label2.Content = label;
        }

        /// <summary>
        /// from left to current
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var currentSelected = from temp in this.current
                                  where temp.IsChecked
                                  select temp;
            List<Data> list = currentSelected.ToList();

            foreach (var temp in list)
            {
                this.current.Remove(temp);
                temp.IsChecked = false;
                this.left.Add(temp);
            }
            
            Refresh();
           

        }

        private void Refresh()
        {
            foreach (var temp in current)
            {
                temp.IsChecked = false;
            }
            foreach (var temp in left)
            {
                temp.IsChecked = false;
            }
            this.listCurrent.ItemsSource = this.current;
            this.listLeft.ItemsSource = this.left;
        }
        
        /// <summary>
        /// from current to left 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            //todo: 001 Get left area selected

            var leftSelected = from temp in this.left
                               where temp.IsChecked
                               select temp;
            List<Data> list = leftSelected.ToList();
            //todo:002:remove left data

            foreach (var temp in list)
            {
                this.left.Remove(temp);//remove left
                temp.IsChecked = false;//set selected false
                this.current.Add(temp);// add temp
            }

            Refresh();
        }


        public delegate void FunctionCliecked(object sender, RelactionEventArgs e);


        public event FunctionCliecked OnOKButtonClicked;

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (OnOKButtonClicked != null)
                OnOKButtonClicked(this, new RelactionEventArgs(this.current, this.left));
        }

        private void cbContent_Click(object sender, RoutedEventArgs e)
        {
        }
    }

    public class Data:System.ComponentModel.INotifyPropertyChanged
    {

        private string id;

        private string text;


        private bool isChecked;

        public string ID
        {
            set
            {
                this.id = value;
                PropertyChange("ID");
            }

            get{return this.id;}
                
        }

        public string Text
        {
            set
            {
                this.text = value;
                PropertyChange("Text");
            }

            get { return this.text; }
        }

        /// <summary>
        ///是否选中
        /// </summary>
        public bool IsChecked
        {
            set
            {
                this.isChecked = value;
                PropertyChange("IsChecked");
            }
            get
            {
                return this.isChecked;
            }
        }


        #region INotifyPropertyChanged 成员

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        private void PropertyChange(string properyName)
        {
            if(PropertyChanged!=null)
            {
                PropertyChanged(this,new System.ComponentModel.PropertyChangedEventArgs(properyName));
            }
        }

        #endregion
    }


    public class RelactionEventArgs : RoutedEventArgs
    {
        public ObservableCollection<Data> current;


        public ObservableCollection<Data> left;


        public RelactionEventArgs(ObservableCollection<Data> current, ObservableCollection<Data> left)
        {
            this.current=current;
            this.left=left;
        }
    }
}
