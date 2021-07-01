using System;
using System.Collections.Generic;
using System.Text;

namespace MyCloudProject.Common
{
    public class ExerimentRequestMessage
    {
        /// <summary>
        /// Identifier used as RowKey of the <see cref="ExperimentResult"/> instance when it is uploaded
        /// </summary>
        public string experimentId { get; set; }

        /// <summary>
        /// Training file name
        /// </summary>
        public string trainingFile { get; set; }

        /// <summary>
        /// Input file name
        /// </summary>
        public string inputFile { get; set; }

        /// <summary>
        /// Output file name
        /// </summary>
        public string outputFile { get; set; }

        /// <summary>
        /// Info provided by person, who requests the execution of this experiment
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Info provided by person, who requests the execution of this experiment
        /// </summary>
        public string description { get; set; }

    }
}
