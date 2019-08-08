using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;
using AFC.WS.UI.RfidRW;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.BR.Maintenance
{
    public class MaintenanceManager
    {
        /// <summary>
        /// 创建唯一管理类对象
        /// </summary>
        private static MaintenanceManager _Instance;

        /// <summary>
        /// 创建唯一管理类对象
        /// </summary>
        public static MaintenanceManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MaintenanceManager();
                }
                return _Instance;
            }
        }
        /// <summary>
        /// 查询库存表中是否已经存在记录
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="partsType"></param>
        /// <param name="partID"></param>
        /// <returns></returns>
        public bool IsExistsReg(string partID)
        {
            bool isExist = false;
            MatainLablePartStore store = DBCommon.Instance.GetModelValue<MatainLablePartStore>(string.Format("select * from matain_lable_part_store where  part_id='{0}'", partID));
            if (store != null && !string.IsNullOrEmpty(store.part_id))
            {
                isExist = true;
            }
            return isExist;
        }
        public MatainLablePartStore GetMatainLableInfo(string partsID)
        {
              return DBCommon.Instance.GetModelValue<MatainLablePartStore>(string.Format("select * from matain_lable_part_store where part_id='{0}'", partsID));
        }
        /// <summary>
        /// 插入库存表中
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public int insertLablePartStore(MatainLablePartStore store)
        {
            int result = DBCommon.Instance.InsertTable(store, "matain_lable_part_store");
            return result;
        }

        /// <summary>
        /// 更新库存表
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public int updateLablePartStore(MatainLablePartStore store)
        {
            int result = DBCommon.Instance.UpdateTable(store, "matain_lable_part_store", new KeyValuePair<string, string>("part_id", store.part_id));
            return result;
        }

        /// <summary>
        /// 查询库存表显示信息
        /// </summary>
        /// <param name="partsID"></param>
        /// <returns></returns>
        public MatainLablePartStore GetPartStoreById(string partsID)
        {
            MatainLablePartStore store = DBCommon.Instance.GetModelValue<MatainLablePartStore>(string.Format("select st.part_id,st.status,st.check_out_operator,st.update_operator,st.update_date,st.update_time,ba.dev_part_cn_name as PART_TYPE_ID,pr.mc_dep_name as PROVIDER_ID from matain_lable_part_store st left join basi_dev_part_id_info ba on st.part_type_id = ba.dev_part_id left join basi_provider_info pr on st.provider_id = pr.provider_id where st.part_id='{0}'", partsID));
            return store;
        }

        /// <summary>
        /// 查询无标签库存显示信息
        /// </summary>
        /// <param name="partsID"></param>
        /// <returns></returns>
        public MaintainNoLablePartStore GetNoLabelPartStoreById(string partsID)
        {
            MaintainNoLablePartStore store = DBCommon.Instance.GetModelValue<MaintainNoLablePartStore>(string.Format("select st.part_id,st.instore_num,st.update_operator,st.update_date,st.update_time,ba.dev_part_cn_name as PART_TYPE_ID,pr.mc_dep_name as PROVIDER_ID from maintain_no_lable_part_store st left join basi_dev_part_id_info ba on st.part_type_id = ba.dev_part_id left join basi_provider_info pr on st.provider_id = pr.provider_id where st.part_id='{0}'", partsID));
            return store;
        }

        /// <summary>
        /// 取得无标签部件的库存情况
        /// </summary>
        /// <param name="partsID"></param>
        /// <returns></returns>
        public MaintainNoLablePartStore GetNoLablePartStore(string partsID)
        {
            return DBCommon.Instance.GetModelValue<MaintainNoLablePartStore>(string.Format("select * from maintain_no_lable_part_store where part_id='{0}'", partsID));
        }

        /// <summary>
        /// 插入无标签部件库存表
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public int insertNoLablePartStore(MaintainNoLablePartStore store)
        {
            int result = DBCommon.Instance.InsertTable(store, "maintain_no_lable_part_store");
            return result;
        }

        /// <summary>
        /// 插入无标签日志表
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int insertNoLableOperLog(MaintainNoLableOperLog log)
        {
            int result = DBCommon.Instance.InsertTable(log, "maintain_no_lable_oper_log");
            return result;
        }

        /// <summary>
        /// 更新无标签日志表
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int updateNoLableOperLog(MaintainNoLableOperLog log)
        {
            List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
            tempList.Add(new KeyValuePair<string, string>("PROVIDER_ID", log.provider_id));
            tempList.Add(new KeyValuePair<string, string>("PART_TYPE_ID", log.part_type_id));
            tempList.Add(new KeyValuePair<string, string>("PART_ID", log.part_id));
            int result = DBCommon.Instance.UpdateTable(log, "maintain_no_lable_oper_log", tempList.ToArray());
            return result;
        }

        /// <summary>
        /// 更新无标签部件库存表
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public int updateNoLablePartStore(MaintainNoLablePartStore store)
        {
            int result = DBCommon.Instance.UpdateTable(store, "maintain_no_lable_part_store", new KeyValuePair<string, string>("part_id", store.part_id));
            return result;
        }


        /// <summary>
        /// 检查操作员是否存在
        /// </summary>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        public bool isNotExistPrivOperatorInfo(string operatorID)
        {
            Boolean isNotExist = true;
            PrivOperatorInfo operatorInfo = DBCommon.Instance.GetModelValue<PrivOperatorInfo>(string.Format("select * from priv_operator_info t where t.operator_id='{0}'", operatorID));
            if (operatorInfo != null && !string.IsNullOrEmpty(operatorInfo.operator_id))
            {
                isNotExist = false;
            }
            return isNotExist;
        }
    }
}
