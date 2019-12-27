using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVBS.Infrastructure
{
    public class JQueryDataTableParam {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string draw { get; set; }


        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int length { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int start { get; set; }

        /// <summary>
        /// Text used for setting
        /// </summary>
        public string[] columns { get; set; }

        /// <summary>
        /// Text used for ordering
        /// </summary>
        public string[] order { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string[] search { get; set; }


    }
}
