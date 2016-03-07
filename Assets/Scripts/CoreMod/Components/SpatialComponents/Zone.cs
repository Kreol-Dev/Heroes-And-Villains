using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	public class Zone
	{
		HashSet<TileHandle> handles = new HashSet<TileHandle> ();

		public event Form.FormDelegate FormAdded;
		public event Form.FormDelegate FormRemoved;
		public event Form.FormDelegate FormUpdated;
		public event ObjectDelegate<Zone> ZoneUpdated;

		HashSet<Form> forms = new HashSet<Form> ();

		public void AttachForm (Form form)
		{
			if (forms.Add (form))
			{
				if (FormAdded != null)
					FormAdded (form);
				Updated (form);
				form.FormUpdated += Updated;
				form.FormUpdated += FormUpdated;
			}
		}

		public void DetachForm (Form form)
		{
			if (forms.Remove (form))
			{
				if (FormRemoved != null)
					FormRemoved (form);
				Updated (form);
				form.FormUpdated -= Updated;
				form.FormUpdated -= FormUpdated;
			}
		}

		void Updated (Form form)
		{
			if (ZoneUpdated != null)
				ZoneUpdated (this);
		}
	}


}
