
// ConvertApple2EamonAdventureMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using EamonDD.Game.Converters.Apple2Eamon;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ConvertApple2EamonAdventureMenu : Menu, IConvertApple2EamonAdventureMenu
	{
		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle("CONVERT APPLE II EAMON ADVENTURE", true);

			Debug.Assert(gEngine.IsAdventureFilesetLoaded());

			var a2eac = new A2EAdventureConverter();

			gOut.Print("Converting an Apple II Eamon adventure requires you to enter several key pieces of data.  This operation clears the in-memory database contents before loading converted records; if data already exists, you may want to abort this process and save the data before continuing.");

			gOut.Print("The Apple II .dsk file containing the adventure should be extracted to a unique Windows folder using the Cider Press utility.  You should always configure to preserve Apple II formats before extracting the disk image; Eamon CS only recognizes the original binary format.");

			gOut.Print("After converting the adventure, you can modify its data to your liking and then save it by exiting EamonDD from the main menu.  Select 'Y' when asked whether you would like to keep it.");

			gOut.Print("The conversion process minimally validates an adventure's data; upon reloading into any Eamon CS program, a thorough validation occurs.  You may find that the data needs manual repairs to get it to load successfully.");

			gOut.Write("{0}Enter the full (absolute) path of the Apple II Eamon adventure folder: ", Environment.NewLine);

			Buf.Clear();

			gOut.WordWrap = false;

			rc = Globals.In.ReadField(Buf, 256, null, '_', '\0', false, null, null, null, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.WordWrap = true;

			a2eac.AdventureFolderPath = Buf.Trim().ToString().Replace('/', '\\');

			gOut.Print("{0}", Globals.LineSep);

			gOut.WriteLine();

			gOut.Write("Loading Apple II Eamon adventure data... ");

			if (!a2eac.LoadAdventure() || !a2eac.ConvertAdventure())
			{
				gOut.WriteLine("failed");

				gOut.Print(a2eac.ErrorMessage);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			gOut.WriteLine("succeeded");

			var a2eAdv = a2eac.Adventure;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("Apple II Eamon adventure:");

			gOut.Print(a2eAdv.Name);

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}Would you like to convert this adventure for use in Eamon CS (Y/N): ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (Buf.Length == 0 || Buf[0] != 'Y')
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			Globals.Module = null;

			Globals.Database.FreeModules();

			var module = Globals.CreateInstance<IModule>(x =>
			{
				x.Uid = Globals.Database.GetModuleUid();

				x.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(a2eAdv.Name.ToLower().Trim(new char[] { ' ', '\"' }).Truncate(Constants.ModNameLen));

				x.Desc = "TODO";

				x.Author = "TODO";

				x.VolLabel = "TODO";

				x.SerialNum = "000";

				x.LastMod = DateTime.Now;

				x.NumDirs = a2eAdv._nd != 6 ? 12 : 6;

				x.NumRooms = a2eAdv._nr;

				x.NumArtifacts = a2eAdv._na;

				x.NumEffects = a2eAdv._ne;

				x.NumMonsters = a2eAdv._nm;
			});

			Globals.Database.AddModule(module);

			Globals.ModulesModified = true;

			Globals.Module = module;





			Globals.Database.FreeEffects();

			for (var i = 0; i < a2eAdv._ne; i++)
			{
				var a2eEffect = a2eAdv.EffectList[i];

				Debug.Assert(a2eEffect != null);

				a2eEffect._text = a2eEffect._text.ToLower().Trim(new char[] { ' ', '\"' });

				a2eEffect._text = Regex.Replace(a2eEffect._text, @"\s+", " ");

				var regex = new Regex(@"(^[a-z])|[?!.]\s+(.)", RegexOptions.ExplicitCapture);

				a2eEffect._text = regex.Replace(a2eEffect._text, s => s.Value.ToUpper());

				if (a2eEffect._text.Length <= 0)
				{
					a2eEffect._text = "UNUSED";
				}

				var effect = Globals.CreateInstance<IEffect>(x =>
				{
					x.Uid = Globals.Database.GetEffectUid();

					x.Desc = a2eEffect._text.Truncate(Constants.EffDescLen);
				});

				Globals.Database.AddEffect(effect);
			}

			Globals.EffectsModified = true;





			Globals.Database.FreeTriggers();

			Globals.TriggersModified = true;

			Globals.Database.FreeScripts();

			Globals.ScriptsModified = true;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("The adventure was successfully converted.");

		Cleanup:

			;
		}

		public ConvertApple2EamonAdventureMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
