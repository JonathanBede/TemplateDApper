using System;
using System.Runtime.Serialization;

namespace Tornado.API.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// A description of the game
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// The type of game
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// (Lower band i.e. 40%) if success rate for a word in a round is below this percentage then it will be added ‘AddWordsBand1’ times to next round
        /// </summary>
        [DataMember]
        public double PercentBandMin { get; set; }

        /// <summary>
        /// (Upper band i.e. 80%) if success rate for a word in a round is below this percentage (and above ‘PercentBand1’) then it will be added ‘AddWordsBand1’ times to next round
        /// </summary>
        [DataMember]
        public double PercentBandMax { get; set; }

        /// <summary>
        /// Number of times a word is added to the next game if success rate is below PercentBand1 i.e. ‘2’
        /// </summary>
        [DataMember]
        public int? AddWordsBand1 { get; set; }

        /// <summary>
        /// Number of times a word is added to the next game if success rate is below PercentBand2 (but above PercentBand2) i.e. ‘1’
        /// </summary>
        [DataMember]
        public int? AddWordsBand2 { get; set; }

        /// <summary>
        /// Lower limit of range in queue to re-insert failed words
        /// </summary>
        [DataMember]
        public int? ReEnterPositionMin { get; set; }

        /// <summary>
        /// Upper limit of range in queue to re-insert failed words
        /// </summary>
        [DataMember]
        public int? ReEnterPositionMax { get; set; }

        public Game()
        {
            
        }
    }
}