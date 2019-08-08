using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AFC.WS.UI.UIPage.CashManager
{
    
    /// <summary>
    /// 现金操作的Detail，为ListView中绑定数据赋值
    /// </summary>
    public class CashDetailsInfo:INotifyPropertyChanged
    {

        private bool isChecked;

        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                this.isChecked = value;
                this.OnPropertyChange("IsChecked");
            }
        }

        private string cashTypeCode;

        /// <summary>
        /// 现金类型代码
        /// </summary>
        public string CashTypeCode
        {
            get { return cashTypeCode; }
            set { cashTypeCode = value;
            this.OnPropertyChange("CashTypeCode");
            }
        }

        private string cashTypeName;

        /// <summary>
        /// 现金类型代码
        /// </summary>
        public string CashTypeName
        {
            get { return cashTypeName; }
            set
            {
                cashTypeName = value;
                this.OnPropertyChange("CashTypeName");
            }
        }

        private double cashNumber;

        /// <summary>
        /// 现金张数
        /// </summary>
        public double CashNumber
        {
            get { return cashNumber; }
            set
            {
                cashNumber = value;
                this.OnPropertyChange("CashNumber");
            }
        }

        private double totalMoneyValue;

        /// <summary>
        /// 总金额
        /// </summary>
        public double TotalMoneyValue
        {
            get { return totalMoneyValue; }
            set
            {
                totalMoneyValue = value;
                this.OnPropertyChange("TotalMoneyValue");
            }
        }

        private  int moneyCodeValue;

        /// <summary>
        /// 单位
        /// </summary>
        public int MoneyCodeValue
        {
            get { return this.moneyCodeValue; }
            set
            {
                this.moneyCodeValue = value;
                this.OnPropertyChange("MoneyCodeValue");
            }
        }

        private string updateDate;

        /// <summary>
        /// 更新日期
        /// </summary>
        public string UpdateDate
        {
            get { return updateDate; }
            set
            {
                updateDate = value;
                this.OnPropertyChange("UpdateDate");
            }
        }

        private string updateTime;

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime
        {
            get { return updateTime; }
            set
            {
                updateTime = value;
                this.OnPropertyChange("UpdateTime");
            }
        }

        private string operatorId;

        /// <summary>
        /// 操作员编码
        /// </summary>
        public string OperatorId
        {
            get { return operatorId; }
            set
            {
                operatorId = value;
                this.OnPropertyChange("OperatorId");
            }

        }


        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
