using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinderellaEditer
{
    class Note
    {
        public int channel;
        public int bar_number;
        public int type;
        public int rythem;
        public int timing;
        public int first;
        public int end;
        public Note()
        {
            this.channel = -1;
            this.bar_number = -1;
            this.type = 0;
            this.rythem = 0;
            this.timing = -1;
            this.first = 0;
            this.end = 0;
        }
        public Note(Note note)
        {
            this.channel = note.channel;
            this.bar_number = note.bar_number;
            this.type = note.type;
            this.rythem = note.rythem;
            this.timing = note.timing;
            this.first = note.first;
            this.end = note.end;
        }
        public Note(int channel, int bar_number, int type, int rythem, int timing, int first, int end)
        {
            this.channel = channel;
            this.bar_number = bar_number;
            this.type = type;
            this.rythem = rythem;
            this.timing = timing;
            this.first = first;
            this.end = end;
        }

        public double GetTiming()
        {
            return (double)timing / (double)rythem;
        }
    }
}
