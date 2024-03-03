
// EditGlobalLaunchParametersMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditGlobalLaunchParametersMenu : Menu, IEditGlobalLaunchParametersMenu
	{
		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle("EDIT GLOBAL LAUNCH PARAMETERS", true);

			Debug.Assert(!gEngine.IsAdventureFilesetLoaded());

			gOut.Print("The global launch parameters file [{0}] contains the command-line parameters passed to all Eamon CS programs when they are run.  The parameters should be space-delimited and contain no spaces.", gEngine.GlobalLaunchParametersFile);

			gOut.Write("{0}Enter the parameters to store in the file: ", Environment.NewLine);

			var origFileText = "";

			try
			{
				origFileText = gEngine.File.ReadAllText(gEngine.GlobalLaunchParametersFile);
			}
			catch (Exception)
			{
				// Do nothing
			}

			origFileText = origFileText.Replace("\r\n", " ").Replace("\n", " ");

			origFileText = Regex.Replace(origFileText, @"\s+", " ").Trim();

			Buf.SetFormat("{0}", origFileText);

			gOut.WordWrap = false;

			rc = gEngine.In.ReadField(Buf, 256, null, '_', '\0', true, null, null, null, null);		// TODO: replace 256 hardcode with const ???

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.WordWrap = true;

			gEngine.Thread.Sleep(150);

			var currFileText = Regex.Replace(Buf.ToString(), @"\s+", " ").Trim();

			if (!currFileText.Equals(origFileText, StringComparison.OrdinalIgnoreCase))
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Would you like to save these parameters to the file (Y/N): ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Saving file ... ", Environment.NewLine);

				try
				{
					gEngine.File.WriteAllText(gEngine.GlobalLaunchParametersFile, currFileText, new ASCIIEncoding());

					gOut.Write("succeeded.{0}", Environment.NewLine);
				}
				catch (Exception ex)
				{
					gOut.Write("failed.{0}", Environment.NewLine);

					if (ex.InnerException != null)      // TODO: verify
					{
						ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
					}
					else
					{
						throw;
					}
				}
			}
			else
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("Parameters not modified.");
			}

		Cleanup:

			;
		}

		public EditGlobalLaunchParametersMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
