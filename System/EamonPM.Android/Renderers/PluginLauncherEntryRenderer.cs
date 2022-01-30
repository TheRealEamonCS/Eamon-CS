
// PluginLauncherEntryRenderer.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Android.Content;
using Android.Views.InputMethods;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Eamon.Mobile;

[assembly: ExportRenderer(typeof(Eamon.Mobile.Models.PluginLauncherEntry), typeof(EamonPM.PluginLauncherEntryRenderer))]

namespace EamonPM
{
	public class PluginLauncherEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				var entry = (TextView)Control;

				entry.ImeOptions = (ImeAction)ImeFlags.NoExtractUi;

				entry.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
				{
					args.Handled = args.ActionId == Android.Views.InputMethods.ImeAction.Done;

					if (args.Handled)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							((IEntryController)Element).SendCompleted();
						});
					}
				};
			}
		}

		public PluginLauncherEntryRenderer(Context context) : base(context)
		{

		}
	}
}