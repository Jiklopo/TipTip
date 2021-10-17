using System;

namespace Events
{
	public static class GameBus
	{
		public static Action OnLevelCompleted;
		public static Action OnGamePaused;
	}
}