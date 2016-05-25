﻿/* The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 * 
 * 
 * The Initial Developer of the Original Code is Callum McGing (mailto:callum.mcging@gmail.com).
 * Portions created by the Initial Developer are Copyright (C) 2013-2016
 * the Initial Developer. All Rights Reserved.
 */
using System;
using OpenTK.Audio.OpenAL;
namespace LibreLancer.Media
{
	public class SoundEffectInstance
	{
		int sid;
		AudioManager au;
		SoundData data;
		internal SoundEffectInstance(AudioManager manager, int source, SoundData data)
		{
			this.sid = source;
			this.au = manager;
			this.data = data;
		}

		public void Play(float volume)
		{
			AudioManager.ALFunc(() => AL.BindBufferToSource(sid, data.ID));
			AudioManager.ALFunc(() => AL.Source(sid, ALSourcef.Gain, volume));
			au.PlayInternal(sid);
		}
	}
}

