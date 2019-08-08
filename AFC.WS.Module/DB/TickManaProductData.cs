using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.DB
{
        //dusj add begin 20121019
        public class TickManaProductData : INotifyPropertyChanged
        {

            private bool isChecked;

            public bool IsChecked
            {
                get { return isChecked; }
                set
                {
                    isChecked = value;
                    PropertyChange("IsChecked");
                }
            }

            private string tickStoreTypeName;

            public string TickStoreTypeName
            {
                get { return tickStoreTypeName; }
                set
                {
                    tickStoreTypeName = value;
                    PropertyChange("TickStoreTypeName");
                }
            }

            private string tickStoreType;

            public string TickStoreType
            {
                get { return tickStoreType; }
                set
                {
                    tickStoreType = value;
                    PropertyChange("TickStoreType");
                }
            }

            private string updateDate;

            public string UpdateDate
            {
                get { return updateDate; }
                set
                {
                    updateDate = value;
                    PropertyChange("UpdateDate");
                }
            }

            private string updateTime;

            public string UpdateTime
            {
                get { return updateTime; }
                set
                {
                    updateTime = value;
                    PropertyChange("UpdateTime");
                }
            }

            private int tickNum;

            public int TickNum
            {
                get { return tickNum; }
                set
                {
                    tickNum = value;
                    PropertyChange("TickNum");
                }
            }

            private int operation;

            public int Operation
            {
                get { return operation; }
                set
                {
                    operation = value;
                    PropertyChange("Operation");
                }
            }

            private string operatorId;

            public string OperatorId
            {
                get { return operatorId; }
                set
                {
                    operatorId = value;
                    PropertyChange("OperatorId");
                }
            }
            private decimal checkInMoney;

            public decimal CheckInMoney
            {
                get { return checkInMoney; }
                set
                {
                    checkInMoney = value;
                    PropertyChange("CheckInMoney");
                }
            }

            #region INotifyPropertyChanged 成员

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion

            private void PropertyChange(string properyTypeName)
            {
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(properyTypeName));
                }
            }
        }
    }
