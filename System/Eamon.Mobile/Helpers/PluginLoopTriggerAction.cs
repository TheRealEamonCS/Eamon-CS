
// PluginLoopTriggerAction.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Xamarin.Forms;

namespace Eamon.Mobile.Helpers
{
	public class PluginLoopTriggerAction : TriggerAction<VisualElement>
	{
		public static bool LaunchedPluginLoop { get; set; }

		protected override void Invoke(VisualElement visual)
		{
			if (!LaunchedPluginLoop)
			{
				LaunchedPluginLoop = true;

				App.StartGameThread();
			}
		}

		public PluginLoopTriggerAction()
		{

		}
	}
}