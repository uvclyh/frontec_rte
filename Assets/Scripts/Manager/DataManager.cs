using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using Noah;




namespace Noah
{

	public interface ILoader<Key, Item>
		{
			Dictionary<Key, Item> MakeDic();
			bool Validate();
		}

		public class DataManager
		{

			public Dictionary<string, Profile> AddressSpaceDatas { get; private set; }
			public Dictionary<string, Project> projectListDatas { get; private set; }

			public void Init()
			{
				// AddressSpaceDatas
				AddressSpaceDatas = LoadJson<AddressSpaceData, string, Profile>("AddressSpaceData").MakeDic();
				
				//projectListDatas = LoadJson<ProjectData, string, Project>("ProjectData").MakeDic();
			}

			private Item LoadSingleXml<Item>(string name)
			{
				XmlSerializer xs = new XmlSerializer(typeof(Item));
				TextAsset textAsset = Resources.Load<TextAsset>("Data/" + name);
				using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
					return (Item)xs.Deserialize(stream);
			}

			private Loader LoadXml<Loader, Key, Item>(string name) where Loader : ILoader<Key, Item>, new()
			{
				XmlSerializer xs = new XmlSerializer(typeof(Loader));
				TextAsset textAsset = Resources.Load<TextAsset>("Data/" + name);
				using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
					return (Loader)xs.Deserialize(stream);
			}


			private Loader LoadJson<Loader, Key, Item>(string path) where Loader : ILoader<Key, Item>
			{
				TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
				return JsonUtility.FromJson<Loader>(textAsset.text);
			}

		}


}
