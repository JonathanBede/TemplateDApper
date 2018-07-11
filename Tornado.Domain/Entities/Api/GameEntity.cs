using System;
using System.Collections.Generic;

namespace Tornado.Domain.Entities.Api
{
    public class GameEntity : BaseEntity
    {
        /// <summary>
        /// Name of the game
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the game
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The type of game
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// (Lower band i.e. 40%) if success rate for a word in a round is below this percentage then it will be added ‘AddWordsBand1’ times to next round
        /// </summary>
        public double PercentBandMin { get; set; }

        /// <summary>
        /// (Upper band i.e. 80%) if success rate for a word in a round is below this percentage (and above ‘PercentBand1’) then it will be added ‘AddWordsBand1’ times to next round
        /// </summary>
        public double PercentBandMax { get; set; }

        /// <summary>
        /// Number of times a word is added to the next game if success rate is below PercentBand1 i.e. ‘2’
        /// </summary>
        public int? AddWordsBand1 { get; set; }

        /// <summary>
        /// Number of times a word is added to the next game if success rate is below PercentBand2 (but above PercentBand2) i.e. ‘1’
        /// </summary>
        public int? AddWordsBand2 { get; set; }

        /// <summary>
        /// Lower limit of range in queue to re-insert failed words
        /// </summary>
        public int? ReEnterPositionMin { get; set; }

        /// <summary>
        /// Upper limit of range in queue to re-insert failed words
        /// </summary>
        public int? ReEnterPositionMax { get; set; }

        public Guid AppId { get; set; }


        public GameEntity()
        {
            
        }
    }
}
