using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MusicMaker
{
    public class Music
    {


        public static void Main(string[] args)
        {
            //notes are in two bar phrases

            var noteList = PlayNote.ReadNotes(
                "a8.f#16d4f#4a4d>2f#>8.e>16" +
                "d>4f#4g#4a2a8a8" +
                "f#>4.e>8d>4c#>2b8.c#>16" +
                "d>4d>4a4f#4d4a8.f#16" +
                "d4f#4a4d>2f#>8.e>16" +
                "d>4f#4g#4a2a8a8" +
                "f#>4.e>8d>4c#>2b8.c#>16" +
                "d>4d>4a4f#4d4f#>8.f#>16" +
                "f#>4g>4a>4a>2g>8f#>8" +
                "e>4f#>4g>4g>2g>4" +
                "f#>4.e>8d>4c#>2b8.c#>16" +
                "d>4f#4g#4a2a4" +
                "d>4d>4d>8c#>8b4b4b4" +
                "e>4g>8f#>8e>8d>8d>4c#>4a8.a16" +
                "d>4.e>8f#>8g>8a>2d>8e>8" +
                "f#>4.g>8e>4d>2");
            if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
            {
                noteList = PlayNote.ReadNotes(args[0]);
            }
            var noteList2 = PlayNote.ReadNotes("d>12d>12d>12d>4a#4c>4d>4*pc>8d>2");

            foreach (Note note in noteList)
            {
                note.Play();
            }

        }
    }
}
