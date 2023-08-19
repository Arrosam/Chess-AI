using System;

namespace Chess {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;

	[CreateAssetMenu (menuName = "AI/Settings")]
	public class AISettings : ScriptableObject {
        

		public int depth;
		public bool useIterativeDeepening;
		public bool useTranspositionTable;
        
		public bool useFixedDepthSearch;
		public int searchTimeMillis = 1000;
		public bool endlessSearchMode;
		public bool clearTTEachMove;
		
		public MoveGenerator.PromotionMode promotionsToSearch;
	}
}