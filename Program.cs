using System;
using System.IO;
using System.IO.Compression;
using aviationLib;

namespace ilsLoader
{
    class Program
    {
        static Morse morse;

        static Char[] recordType_001_04 = new Char[04];
        static Char[] facilityId_160_04 = new Char[04];

        static Char[] runway_016_03 = new Char[03];
        static Char[] type_019_10 = new Char[10];
        static Char[] idCode_029_06 = new Char[06];
        static Char[] category_173_09 = new Char[09];
        static Char[] approachBearing_282_06 = new Char[06];
        static Char[] magVar_288_03 = new Char[03];

        static Char[] latitude_061_14 = new Char[14];
        static Char[] longitude_086_14 = new Char[14];
        static Char[] altitudeMsl_127_07 = new Char[07];
        static Char[] frequency_134_07 = new Char[07];
        static Char[] backcourse_141_15 = new Char[15];
        static Char[] courseWidth_156_05 = new Char[05];
        static Char[] courseWidthThreshold_161_07 = new Char[07];

        static Char[] gsType_134_15 = new Char[15];
        static Char[] gsAngle_149_05 = new Char[05];
        static Char[] gsFreq_154_07 = new Char[07];

        static Char[] status_029_22 = new Char[22];
        static Char[] distance_113_07 = new Char[07];

        static Char[] markerType_029_02 = new Char[02];
        static Char[] status_031_22 = new Char[22];
        static Char[] distance_115_07 = new Char[07];
        static Char[] facilityType_136_15 = new Char[15];
        static Char[] locationIdent_151_02 = new Char[02];
        static Char[] markerName_153_30 = new Char[30];
        static Char[] markerFreq_183_03 = new Char[03];
        static Char[] relation_186_25 = new Char[25];
        static Char[] ndbStatus_211_22 = new Char[22];
        static Char[] service_233_30 = new Char[30];

        static Char[] remark_029_350 = new Char[350];

        static StreamWriter ofileILS1 = new StreamWriter("ilsApproach.txt");
        static StreamWriter ofileILS2 = new StreamWriter("ilsFrequency.txt");
        static StreamWriter ofileILS3 = new StreamWriter("ilsGlideslope.txt");
        static StreamWriter ofileILS4 = new StreamWriter("ilsDme.txt");
        static StreamWriter ofileILS5 = new StreamWriter("ilsMarker.txt");
        static StreamWriter ofileILS6 = new StreamWriter("ilsRemarks.txt");

        static void Main(String[] args)
        {
            String userprofileFolder = Environment.GetEnvironmentVariable("USERPROFILE");
            String[] fileEntries = Directory.GetFiles(userprofileFolder + "\\Downloads\\", "28DaySubscription*.zip");

            ZipArchive archive = ZipFile.OpenRead(fileEntries[0]);
            ZipArchiveEntry entry = archive.GetEntry("ILS.txt");
            entry.ExtractToFile("ILS.txt", true);

            StreamReader file = new StreamReader("ILS.txt");

            morse = new Morse();

            String rec = file.ReadLine();

            while (!file.EndOfStream)
            {
                ProcessRecord(rec);
                rec = file.ReadLine();
            }

            ProcessRecord(rec);

            file.Close();

            ofileILS1.Close();
            ofileILS2.Close();
            ofileILS3.Close();
            ofileILS4.Close();
            ofileILS5.Close();
            ofileILS6.Close();
        }

        static void ProcessRecord(String record)
        {
            recordType_001_04 = record.ToCharArray(0, 4);

            String rt = new String(recordType_001_04);

            Int32 r = String.Compare(rt, "ILS1");
            if (r == 0)
            {
                facilityId_160_04 = record.ToCharArray(159, 4);
                String s = new String(facilityId_160_04).Trim();
                ofileILS1.Write(s);
                ofileILS1.Write('~');

                runway_016_03 = record.ToCharArray(15, 3);
                s = new String(runway_016_03).Trim();
                ofileILS1.Write(s);
                ofileILS1.Write('~');

                type_019_10 = record.ToCharArray(18, 10);
                s = new String(type_019_10).Trim();
                ofileILS1.Write(s);
                ofileILS1.Write('~');

                idCode_029_06 = record.ToCharArray(28, 6);
                s = new String(idCode_029_06).Trim();
                ofileILS1.Write(s);
                ofileILS1.Write('~');

                Morse.LetterCode lc = morse.list.Find(x => x.letter.Contains(idCode_029_06[0].ToString()));
                if (lc.code != null)
                {
                    ofileILS1.Write(lc.code);
                }

                lc = morse.list.Find(x => x.letter.Contains(idCode_029_06[2].ToString()));
                if (lc.code != null)
                {
                    ofileILS1.Write(' ');
                    ofileILS1.Write(lc.code);
                }

                lc = morse.list.Find(x => x.letter.Contains(idCode_029_06[3].ToString()));
                if (lc.code != null)
                {
                    ofileILS1.Write(' ');
                    ofileILS1.Write(lc.code);
                }

                lc = morse.list.Find(x => x.letter.Contains(idCode_029_06[4].ToString()));
                if (lc.code != null)
                {
                    ofileILS1.Write(' ');
                    ofileILS1.Write(lc.code);
                }

                lc = morse.list.Find(x => x.letter.Contains(idCode_029_06[5].ToString()));
                if (lc.code != null)
                {
                    ofileILS1.Write(' ');
                    ofileILS1.Write(lc.code);
                }

                ofileILS1.Write('~');

                category_173_09 = record.ToCharArray(172, 9);
                s = new String(category_173_09).Trim();
                ofileILS1.Write(s);
                ofileILS1.Write('~');

                approachBearing_282_06 = record.ToCharArray(281, 6);
                s = new String(approachBearing_282_06).Trim();
                s = s.TrimStart('0');
                ofileILS1.Write(s);
                ofileILS1.Write('~');

                magVar_288_03 = record.ToCharArray(287, 3);
                
                s = new String(magVar_288_03).Trim();

                MagVar mv = new MagVar(s);

                ofileILS1.Write(mv.magVar.ToString("F2"));
                ofileILS1.Write(ofileILS1.NewLine);
            }

            r = String.Compare(rt, "ILS2");
            if (r == 0)
            {
                String s = new String(facilityId_160_04).Trim();
                ofileILS2.Write(s);
                ofileILS2.Write('~');

                s = new String(runway_016_03).Trim();
                ofileILS2.Write(s);
                ofileILS2.Write('~');

                latitude_061_14 = record.ToCharArray(60, 14);
                s = new String(latitude_061_14).Trim();
                ofileILS2.Write(s);
                ofileILS2.Write('~');

                longitude_086_14 = record.ToCharArray(85, 14);
                s = new String(longitude_086_14).Trim();
                ofileILS2.Write(s);
                ofileILS2.Write('~');

                altitudeMsl_127_07 = record.ToCharArray(126, 7);
                s = new String(altitudeMsl_127_07).Trim();
                ofileILS2.Write(s);
                ofileILS2.Write('~');

                frequency_134_07 = record.ToCharArray(133, 7);
                s = new String(frequency_134_07).Trim().PadRight(7, '0');
                ofileILS2.Write(s);
                ofileILS2.Write('~');

                backcourse_141_15 = record.ToCharArray(140, 15);
                s = new String(backcourse_141_15).Trim();
                ofileILS2.Write(s);
                ofileILS2.Write('~');

                courseWidth_156_05 = record.ToCharArray(155, 5);
                s = new String(courseWidth_156_05).Trim();
                ofileILS2.Write(s);
                ofileILS2.Write('~');

                courseWidthThreshold_161_07 = record.ToCharArray(160, 7);
                s = new String(courseWidthThreshold_161_07).Trim();
                ofileILS2.Write(s);
                ofileILS2.Write(ofileILS2.NewLine);
            }

            r = String.Compare(rt, "ILS3");
            if (r == 0)
            {
                String s = new String(facilityId_160_04).Trim();
                ofileILS3.Write(s);
                ofileILS3.Write('~');

                s = new String(runway_016_03).Trim();
                ofileILS3.Write(s);
                ofileILS3.Write('~');

                latitude_061_14 = record.ToCharArray(60, 14);
                s = new String(latitude_061_14).Trim();
                ofileILS3.Write(s);
                ofileILS3.Write('~');

                longitude_086_14 = record.ToCharArray(85, 14);
                s = new String(longitude_086_14).Trim();
                ofileILS3.Write(s);
                ofileILS3.Write('~');

                altitudeMsl_127_07 = record.ToCharArray(126, 7);
                s = new String(altitudeMsl_127_07).Trim();
                ofileILS3.Write(s);
                ofileILS3.Write('~');

                gsType_134_15 = record.ToCharArray(133, 15);
                s = new String(gsType_134_15).Trim();
                ofileILS3.Write(s);
                ofileILS3.Write('~');

                gsAngle_149_05 = record.ToCharArray(148, 5);
                s = new String(gsAngle_149_05).Trim();
                ofileILS3.Write(s);
                ofileILS3.Write('~');

                gsFreq_154_07 = record.ToCharArray(153, 7);
                s = new String(gsFreq_154_07).Trim().PadRight(7, '0');
                ofileILS3.Write(s);
                ofileILS3.Write(ofileILS3.NewLine);
            }

            r = String.Compare(rt, "ILS4");
            if (r == 0)
            {
                String s = new String(facilityId_160_04).Trim();
                ofileILS4.Write(s);
                ofileILS4.Write('~');

                s = new String(runway_016_03).Trim();
                ofileILS4.Write(s);
                ofileILS4.Write('~');

                status_029_22 = record.ToCharArray(28, 22);
                s = new String(status_029_22).Trim();
                ofileILS4.Write(s);
                ofileILS4.Write('~');

                distance_113_07 = record.ToCharArray(112, 7);
                s = new String(distance_113_07).Trim();
                ofileILS4.Write(s);
                ofileILS4.Write(ofileILS4.NewLine);
            }

            r = String.Compare(rt, "ILS5");
            if (r == 0)
            {
                String s = new String(facilityId_160_04).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write('~');

                s = new String(runway_016_03).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write('~');

                markerType_029_02 = record.ToCharArray(28, 2);
                s = new String(markerType_029_02).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write('~');

                status_031_22 = record.ToCharArray(30, 22);
                s = new String(status_031_22).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write('~');

                distance_115_07 = record.ToCharArray(114, 7);
                s = new String(distance_115_07).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write('~');

                facilityType_136_15 = record.ToCharArray(135, 15);
                s = new String(facilityType_136_15).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write('~');

                locationIdent_151_02 = record.ToCharArray(150, 2);
                s = new String(locationIdent_151_02).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write('~');

                Morse.LetterCode lc = morse.list.Find(x => x.letter.Contains(locationIdent_151_02[0].ToString()));
                if (lc.code != null)
                {
                    ofileILS5.Write(lc.code);
                }

                lc = morse.list.Find(x => x.letter.Contains(locationIdent_151_02[1].ToString()));
                if (lc.code != null)
                {
                    ofileILS5.Write(' ');
                    ofileILS5.Write(lc.code);
                }

                ofileILS5.Write('~');

                markerName_153_30 = record.ToCharArray(152, 30);
                s = new String(markerName_153_30).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write('~');

                markerFreq_183_03 = record.ToCharArray(182, 3);
                s = new String(markerFreq_183_03).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write('~');

                relation_186_25 = record.ToCharArray(185, 25);
                s = new String(relation_186_25).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write('~');

                ndbStatus_211_22 = record.ToCharArray(210, 22);
                s = new String(ndbStatus_211_22).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write('~');

                service_233_30 = record.ToCharArray(232, 30);
                s = new String(service_233_30).Trim();
                ofileILS5.Write(s);
                ofileILS5.Write(ofileILS5.NewLine);
            }

            r = String.Compare(rt, "ILS6");
            if (r == 0)
            {
                String s = new String(facilityId_160_04).Trim();
                ofileILS6.Write(s);
                ofileILS6.Write('~');

                s = new String(runway_016_03).Trim();
                ofileILS6.Write(s);
                ofileILS6.Write('~');

                remark_029_350 = record.ToCharArray(28, 350);
                s = new String(remark_029_350).Trim();
                ofileILS6.Write(s);
                ofileILS6.Write(ofileILS6.NewLine);
            }
        }

    }

}
