using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.Maintenance
{
    public class DisplayNoLabelParts
    {
        private string _part_id;
        string _mc_dep_name;
        string _dev_part_cn_name;
        string _instore_num;
        string _update_operator;

        public string part_id
        {
            get { return _part_id; }
            set { _part_id = value; }
        }
        public string mc_dep_name
        {
            get { return _mc_dep_name; }
            set { _mc_dep_name = value; }
        }
        public string dev_part_cn_name
        {
            get { return _dev_part_cn_name; }
            set { _dev_part_cn_name = value; }
        }
        public string instore_num
        {
            get { return _instore_num; }
            set { _instore_num = value; }
        }
        public string update_operator
        {
            get { return _update_operator; }
            set { _update_operator = value; }
        }
    }
}
