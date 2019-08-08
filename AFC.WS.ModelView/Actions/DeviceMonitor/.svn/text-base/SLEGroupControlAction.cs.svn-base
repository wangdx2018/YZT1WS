using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.BR;
using AFC.WS.BR.DeviceMonitor;
using AFC.WS.Model.DB;
using TJComm;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using AFC.WS.Model.Const;


namespace AFC.WS.ModelView.Actions.DeviceMonitor
{
    public  class SLEGroupControlAction: IAction
    {
        private string commandType=string.Empty;
        private string deviceRange = string.Empty;
        private string stationID = string.Empty;
        private string deviceID = string.Empty;
        private string hallID = string.Empty;
        private string groupID = string.Empty;
        private string devTypeID = string.Empty;
        private Dictionary<string, List<string>> dict = new Dictionary<string,List<string>>();
        private Dictionary<string, string> deviceTypedict = new Dictionary<string, string>();
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("deviceRange")).value != null)
            {
                deviceRange = actionParamsList.Single(temp => temp.bindingData.Equals("deviceRange")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("deviceID")).value != null)
            {
                deviceID = actionParamsList.Single(temp => temp.bindingData.Equals("deviceID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("hallID")).value != null)
            {
                hallID = actionParamsList.Single(temp => temp.bindingData.Equals("hallID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("groupID")).value != null)
            {
                groupID = actionParamsList.Single(temp => temp.bindingData.Equals("groupID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("devTypeID")).value != null)
            {
                devTypeID = actionParamsList.Single(temp => temp.bindingData.Equals("devTypeID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("stationID")).value != null)
            {
                stationID = actionParamsList.Single(temp => temp.bindingData.Equals("stationID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("commandType")).value != null)
            {
                commandType = actionParamsList.Single(temp => temp.bindingData.Equals("commandType")).value.ToString();
            }
            dict = (Dictionary<string, List<string>>)actionParamsList.Single(temp => temp.bindingData.Equals("reSendDict")).value;
            deviceTypedict = (Dictionary<string, string>)actionParamsList.Single(temp => temp.bindingData.Equals("devTypeDict")).value;

            //重发控制命令
            if (deviceRange == "4")
            {
                if (dict == null || dict.Count == 0)
                {
                    MessageDialog.Show("请选择重发控制命令的设备!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                if (!(deviceTypedict==null))
                {
                    //设备类型大于1
                    if (deviceTypedict.Count > 1)
                    {
                        //设备类型不是通用设备
                        if (!(commandType.Substring(0, 2) == "01"))
                        {
                            MessageDialog.Show("请选择相同的设备类型!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            return false;
                        }
                    }
                    else
                    {
                        string devType = null;
                        foreach (KeyValuePair<string, string> dvp in deviceTypedict)
                        {
                           devType = dvp.Key;
                        }
                        //TVM
                        if (devType == "01")
                            if (commandType.Substring(0, 2) == "04" || commandType.Substring(0, 2) == "02")
                            {
                                MessageDialog.Show("请指定与设备类型匹配的控制命令!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                                return false;
                            }
                        //BOM
                        if (devType == "02")
                        {
                            if (commandType.Substring(0, 2) == "02" || commandType.Substring(0, 2) == "03")
                            {
                                MessageDialog.Show("请指定与设备类型匹配的控制命令!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                                return false;
                            }
                        }
                        //AG
                        if (devType == "06")
                        {
                            if (commandType.Substring(0, 2) == "03" || commandType.Substring(0, 2) == "04")
                            {
                                MessageDialog.Show("请指定与设备类型匹配的控制命令!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                                return false;
                            }
                        }

                    }
                }

            }
            //单个设备
            if (deviceRange == "3")
            {
                if (string.IsNullOrEmpty(stationID))
                {
                    MessageDialog.Show("请选择车站!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                if (string.IsNullOrEmpty(deviceID))
                {
                    MessageDialog.Show("请选择单个设备!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                if (!string.IsNullOrEmpty(devTypeID))
                {
                    //TVM
                    if (devTypeID == "01")
                        if (commandType.Substring(0, 2) == "04" || commandType.Substring(0, 2) == "02")
                        {
                            MessageDialog.Show("请指定与设备类型匹配的控制命令!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            return false;
                        }
                    //BOM
                    if (devTypeID == "02")
                    {
                        if (commandType.Substring(0, 2) == "02" || commandType.Substring(0, 2) == "03")
                        {
                            MessageDialog.Show("请指定与设备类型匹配的控制命令!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            return false;
                        }

                    }
                }

            }
            //部份设备
            if (deviceRange == "2")
            {
                if (string.IsNullOrEmpty(stationID))
                {
                    MessageDialog.Show("请选择车站!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                if (string.IsNullOrEmpty(hallID) && string.IsNullOrEmpty(groupID) && string.IsNullOrEmpty(devTypeID))
                {
                    MessageDialog.Show("请指定范围设备!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                if (!string.IsNullOrEmpty(devTypeID))
                {
                    //TVM
                    if(devTypeID=="01")
                        if (commandType.Substring(0, 2) == "04" || commandType.Substring(0, 2) == "02")
                        {
                            MessageDialog.Show("请指定与设备类型匹配的控制命令!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            return false;
                        }
                    //BOM
                    if (devTypeID == "02")
                    {
                        if (commandType.Substring(0, 2) == "02" || commandType.Substring(0, 2) == "03")
                        {
                            MessageDialog.Show("请指定与设备类型匹配的控制命令!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            return false;
                        }

                    }
                }
            }
            //全站设备
            if (deviceRange == "1")
            {
                if (string.IsNullOrEmpty(stationID))
                {
                    MessageDialog.Show("请选择车站!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                
            }

            if (string.IsNullOrEmpty(commandType))
            {
                MessageDialog.Show("请选择要发送的控制命令!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }


            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            List<TJComm.DeviceRange> deviceRange = GetDeviceRange();
            byte controlType = Convert.ToByte(commandType.Substring(0, 2));
            int res = BuinessRule.GetInstace().commProcess.ControlCmd(controlType, commandType.ConvertHexStringToUshort(), deviceRange);
            string commandEn = SLEGroupControlManager.Instance.GetComandEn(commandType.ConvertHexStringToUint());
            string commandOperationCode = SLEGroupControlManager.Instance.GetComandsOperationCode(commandType.ConvertHexStringToUint());
            if (res == 0)
            {
                MessageDialog.Show("控制命令发送成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(commandOperationCode, "0", commandEn + "控制命令发送成功");
            }
            else
            {
                MessageDialog.Show("控制命令发送失败!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(commandOperationCode, "1", commandEn + "控制命令发送失败");
            }    
            return null;
        }

        private List<TJComm.DeviceRange> GetDeviceRange()
        {
            try
            {
                List<TJComm.DeviceRange> list = new List<TJComm.DeviceRange>();

                //全线
                if (deviceRange == "0")
                {
                    list.Add(new TJComm.DeviceRange { special_flag = 0, stationId = 0, deviceRange = new List<uint>() });
                }

                //全站
                if (deviceRange == "1")
                {
                    list.Add(new TJComm.DeviceRange { special_flag = 1, stationId = this.stationID.ConvertHexStringToUshort(), deviceRange = new List<uint>() });
                }

                //部份设备
                if (deviceRange == "2")
                {
                    List<BasiDevInfo> basiDev = SLEGroupControlManager.Instance.GetPartBasiDevInfo(hallID, groupID, devTypeID, stationID);
                    DeviceRange dr = new DeviceRange();
                    dr.special_flag = 2;
                    dr.stationId = stationID.ConvertHexStringToUshort();
                    if (basiDev != null)
                    {
                        foreach (var temp in basiDev)
                        {
                            dr.deviceRange.Add(temp.device_id.ConvertHexStringToUint());
                        }
                    }

                    list.Add(dr);

                }

                //单个设备
                if (deviceRange == "3")
                {
                    if ("LCWS".Equals(SysConfig.GetSysConfig().LocalParamsConfig.SystemName))
                    {
                        stationID = BuinessRule.GetInstace().GetBasiDevInfo(null).Single(temp => temp.device_id == deviceID).station_id;
                    }
                   DeviceRange dr = new DeviceRange();
                   dr.special_flag = 2;
                   dr.stationId = stationID.ConvertHexStringToUshort();
                   dr.deviceRange.Add(this.deviceID.ConvertHexStringToUint());
                   list.Add(dr);

                }

                //重发指定
                if (deviceRange == "4")
                {
                    if (dict==null || dict.Count >0)
                    {
                        foreach (KeyValuePair<string, List<string>> kvp in dict)
                        {
                            DeviceRange dr = new DeviceRange();
                            dr.special_flag = 2;
                            dr.stationId = kvp.Key.ConvertHexStringToUshort();
                            List<uint> deviceTmpRange = new List<uint>();
                            List<string> deviceList = kvp.Value;
                      
                            foreach (string tempDev in deviceList)
                            {
                                deviceTmpRange.Add(tempDev.ConvertHexStringToUint());
                            }
                            dr.deviceRange = deviceTmpRange;
                            list.Add(dr);
                        }

                    }
                        
    
                }

                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置范围异常：" + ex.ToString());
                return null;
            }
            
        }    

        #endregion
    }
}
