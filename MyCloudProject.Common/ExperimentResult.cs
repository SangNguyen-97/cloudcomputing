using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MyCloudProject.Common
{
    public class ExperimentResult : TableEntity
    {
        public ExperimentResult(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
            this.ExperimentId = rowKey;
        }

        public string ExperimentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartTimeUtc { get; set; }

        public DateTime EndTimeUtc { get; set; }

        public long DurationSec { get; set; }

        public string trainingFileUri { get; set; }

        public string inputFileUri { get; set; }

        public string outputFileUri { get; set; }

        public string asString()
        {
            string s = "{\n" +
                "\"ExperimentId\" : \"" + ExperimentId + "\",\n" +
                "\"Name\" : \"" + Name + "\",\n" +
                "\"Description\" : \"" + Description + "\",\n" +
                "\"StartTimeUtc\" : \"" + StartTimeUtc.ToString() + "\",\n" +
                "\"EndTimeUtc\" : \"" + EndTimeUtc.ToString() + "\",\n" +
                "\"DurationSec\" : \"" + DurationSec + "\",\n" +
                "\"trainingFileUri\" : \"" + trainingFileUri + "\",\n" +
                "\"inputFileUri\" : \"" + inputFileUri + "\",\n" +
                "\"outputFileUri\" : \"" + outputFileUri + "\",\n" +
                "\"PartitionKey\" : \"" + this.PartitionKey + "\",\n" +
                "\"RowKey\" : \"" + this.RowKey + "\",\n" +
                "\"Timestamp\" : \"" + this.Timestamp.ToString() + "\",\n" +
                "\"ETag\" : \"" + this.ETag + "\"\n}\n";
            return s;
        }
    }
}
