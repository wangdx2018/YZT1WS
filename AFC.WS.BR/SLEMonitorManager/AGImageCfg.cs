using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace AFC.WS.BR.SLEMonitorManager
{
    
    public class AGImageCfg:ImageCfg
    {
        private string dirt;

        public string Dirt
        {
            set { this.dirt = value; }
            get { return this.dirt; }
        }

      
    }
}
