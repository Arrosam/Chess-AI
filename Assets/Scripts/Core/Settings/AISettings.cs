using System;

namespace Chess {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;

	[CreateAssetMenu (menuName = "Settings/AI")]
	public class AISettings : ScriptableObject {
        

		public int depth;
		public bool useIterativeDeepening;
		public bool useTranspositionTable;
        
		public bool useFixedDepthSearch;
		public int searchTimeMillis = 1000;
		
		public MoveGenerator.PromotionMode promotionsToSearch;
	}
}