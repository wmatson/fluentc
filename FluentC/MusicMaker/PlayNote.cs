using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MusicMaker
{
    static class PlayNote
    {
        private static Dictionary<string, int> _noteDefs;
        static PlayNote(){
            _noteDefs = new Dictionary<string, int>();

            //base range
            _noteDefs.Add("c", 262);
            _noteDefs.Add("c#", 278);
            _noteDefs.Add("d", 294);
            _noteDefs.Add("d#", 311);
            _noteDefs.Add("e", 330);
            _noteDefs.Add("f", 349);
            _noteDefs.Add("f#", 370);
            _noteDefs.Add("g", 392);
            _noteDefs.Add("g#", 415);
            _noteDefs.Add("a", 440);
            _noteDefs.Add("a#", 466);
            _noteDefs.Add("b", 494);
            //end base range

            //upper range
            _noteDefs.Add("c>", 523);
            _noteDefs.Add("c#>", 554);
            _noteDefs.Add("d>", 587);
            _noteDefs.Add("d#>", 622);
            _noteDefs.Add("e>", 659);
            _noteDefs.Add("f>", 699);
            _noteDefs.Add("f#>", 740);
            _noteDefs.Add("g>", 784);
            _noteDefs.Add("g#>", 831);
            _noteDefs.Add("a>", 880);
            _noteDefs.Add("a#>", 932);
            _noteDefs.Add("b>", 988);
            //end upper range

            //lower range
            _noteDefs.Add("c<", 131);
            _noteDefs.Add("c#<", 139);
            _noteDefs.Add("d<", 147);
            _noteDefs.Add("d#<", 156);
            _noteDefs.Add("e<", 165);
            _noteDefs.Add("f<", 175);
            _noteDefs.Add("f#<", 185);
            _noteDefs.Add("g<", 196);
            _noteDefs.Add("g#<", 208);
            _noteDefs.Add("a<", 220);
            _noteDefs.Add("a#<", 233);
            _noteDefs.Add("b<", 247);
            //end lower range

        }

        public static List<Note> ReadNotes(string input)
        {
            List<Note> returnList = new List<Note>();

            Regex regex = new Regex("([abcdefg][#]?[<>]?)(\\d+)([.]?)([*]?)");
            MatchCollection matches = regex.Matches(input);
            foreach(Match matchedNote in matches){
                GroupCollection groups = matchedNote.Groups;
                returnList.Add(new Note(_noteDefs[(groups[1].ToString())],
                    int.Parse(groups[2].ToString()), 
                    !String.IsNullOrEmpty(groups[3].ToString()),
                    !String.IsNullOrEmpty(groups[4].ToString())));                           
            }

            return returnList;
        }


    }

    class Note
    {
        public int Frequency { get; private set; }
        public int Beat{get; private set;}
        public bool Dotted{get; private set;}
        public bool Staccato{get; private set;}

        public Note(int freq, int beatFraction, bool dotted = false, bool staccato = false){
            Frequency = freq;
            Beat = beatFraction;
            Dotted = dotted;
            Staccato = staccato;
        }

        public void Play(){
            int duration = 1600 / Beat;
            //Console.WriteLine(duration);
            if (Dotted)
            {
                duration = (3 * duration) / 2;
            }
            if (Staccato)
            {
                Console.Beep(Frequency, duration/2);
                Thread.Sleep(duration / 2);

            }
            else
                Console.Beep(Frequency, duration);
        }
    }
}
