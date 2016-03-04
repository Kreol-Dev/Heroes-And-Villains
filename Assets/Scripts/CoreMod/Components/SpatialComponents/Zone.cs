using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	public class Zone
	{

		public event Form.FormDelegate FormAdded;
		public event Form.FormDelegate FormRemoved;
		public event Form.FormDelegate FormUpdated;

		HashSet<Form> forms = new HashSet<Form> ();

		public void AttachForm (Form form)
		{
			if (forms.Add (form))
			{
				if (FormAdded != null)
					FormAdded (form);
				form.FormUpdated += FormUpdated;
			}
		}

		public void DetachForm (Form form)
		{
			if (forms.Remove (form))
			{
				if (FormRemoved != null)
					FormRemoved (form);
				form.FormUpdated += FormUpdated;
			}
		}
	}


}
