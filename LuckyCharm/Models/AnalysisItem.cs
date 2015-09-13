using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LuckyCharm.Models
{
    public class AnalysisItem
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public int PrimaryNumber { get; set; }

        [Description("Number of days has past since last occurrence of this number.")]
        public int LengthToPrevOccur { get; set; }

        [Description("Average of all the lengths in days between occurrences of this number, excluded current length.")]
        public int AvrOfPrevLengths { get; set; }

        [Description("Delta of current length compared to previous length.")]
        public int DeltaToPreviousLength { get; set; }

        [Description("Delta of current length compared to average of all the lengths.")]
        public int DeltaToAvrOfPrevLengths { get; set; }

        [Description("Average of all delta of current length compared to previous length.")]
        public int AvrOfDeltaToAvrOfPrevLengths { get; set; }

        [Description("Average of all the lengths in days between occurrences of this number, included current length.")]
        public int CurrentAvrOfLengths { get; set; }

        [Description("Count of lengths that is smaller than the average of max length and min length.")]
        public int NumberOfLows { get; set; }

        [Description("Percentage of total of all the low lengths over total of all the lengths.")]
        public int PercentageOfLows { get; set; }

        [Description("Average of all the low lengths.")]
        public int AvrOfLows { get; set; }

        [Description("Count of lengths that is greater than min length + 1/4 of (max length - min length), ans smaller than max length - 1/4 of (max length - min length).")]
        public int NumberOfMedium { get; set; }

        [Description("Percentage of total of all the medium lengths over total of all the lengths.")]
        public int PercentageOfMediums { get; set; }

        [Description("Average of all the medium lengths.")]
        public int AvrOfMediums { get; set; }

        [Description("Count of lengths that is higher than the average of max length and min length.")]
        public int NumberOfHighs { get; set; }

        [Description("Percentage of total of all the high lengths over total of all the lengths.")]
        public int PercentageOfHighs { get; set; }

        [Description("Average of all the high lengths.")]
        public int AvrOfHighs { get; set; }
    }
}