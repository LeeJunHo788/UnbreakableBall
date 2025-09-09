using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class AugmentChoiceUI : MonoBehaviour
{
		[Serializable]
		public class OptionWidgets
		{
				public Button button;
				public Image icon;
				public TextMeshProUGUI title;
				public TextMeshProUGUI desc;
		}

		[Header("¿É¼Ç ½½·Ô")]
		public OptionWidgets[] options = new OptionWidgets[3];

		private Action<AugmentDefinition> _onPick;

		public void Show(List<AugmentDefinition> defs, Action<AugmentDefinition> onPick)
		{
				gameObject.SetActive(true);
				_onPick = onPick;

				for (int i = 0; i < options.Length; i++)
				{
						var w = options[i];

						if (i < defs.Count)
						{
								var d = defs[i];
								if (w.title) w.title.text = d.displayName;
								if (w.desc) w.desc.text = d.description;
								if (w.icon) w.icon.sprite = d.icon;

								w.button.onClick.RemoveAllListeners();
								w.button.onClick.AddListener(() => _onPick?.Invoke(d));
								w.button.gameObject.SetActive(true);
						}
						else
						{
								w.button.gameObject.SetActive(false);
						}
				}
		}

		public void Hide()
		{
				foreach (var w in options)
						w.button.onClick.RemoveAllListeners();
				gameObject.SetActive(false);
		}
}