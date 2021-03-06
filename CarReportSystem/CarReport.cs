﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarReportSystem {
    [Serializable]
    class CarReport {
        
        public DateTime CreatedDate { get; set; }

        public string Author { get; set; }

        public CarMaker Maker { get; set; }

        public string CarName { get; set; }

        public string Report { get; set; }

        public Image imgPicture { get; set; }

        public enum CarMaker
        {
            DEFAULT,
            トヨタ,
            日産,
            ホンダ,
            スバル,
            外車,
            その他
        }
    }
}
