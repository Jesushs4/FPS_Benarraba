using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[ExecuteInEditMode]
public class RunThingsInEditorScript : MonoBehaviour
{
	//Variables
	private bool areActiveBuilding = true;
	private bool areActiveProps = true;
	GameObject[] allBuildings;
	GameObject[] allProps;


	//Toggle on or off all the buildings and props
	[Button]
	public void ToggleBuildings()
	{
		if (allBuildings != null && allBuildings.Length > 0)
        {
			areActiveBuilding = !areActiveBuilding;

			PlayerController playerController = FindObjectOfType<PlayerController>();

			foreach (GameObject building in playerController.hiddenBuildings)
			{
				//building.SetActive(areActive);

                for (int i = 0; i < building.transform.childCount; i++)
                {
					Debug.Log(building.transform.GetChild(i).name);
					building.transform.GetChild(i).GetComponentInChildren<MeshRenderer>().enabled = areActiveBuilding;
				}

				if(building.GetComponent<MeshRenderer>())
					building.GetComponent<MeshRenderer>().enabled = areActiveBuilding;

			}
		}
        else
        {
			Debug.LogWarning("NO HAY CASAS NI OBJETOS, ASEGÚRATE DE PULSAR EL BOTÓN FINDBUILDINGS");
        }
	}


	//Find all the buildings and put them in the player controller
	[Button]
	public void FindBuildings()
	{
		//allBuildings = GameObject.FindGameObjectsWithTag("Building");

		//Find objects, including inactive ones by tag
		allBuildings = GameObject.FindObjectsOfType<GameObject>(true).Where(x => x.CompareTag("Building")).ToArray();

		PlayerController playerController = FindObjectOfType<PlayerController>();

		if (allBuildings.Length > 0) playerController.hiddenBuildings = allBuildings;
		else
        {
			Debug.LogWarning("NO SE HAN PODIDO ENCONTRAR CASAS NI OBJETOS, ASEGÚRATE DE QUE TENGAN LA ETIQUETA ASIGNADA");
        }
	}

	[Button]
	public void FindProps()
	{
		//allBuildings = GameObject.FindGameObjectsWithTag("Building");

		//Find objects, including inactive ones by tag
		allProps = GameObject.FindObjectsOfType<GameObject>(true).Where(x => x.CompareTag("Props")).ToArray();

		PlayerController playerController = FindObjectOfType<PlayerController>();

		if (allProps.Length > 0) playerController.hiddenProps = allProps;
		else
		{
			Debug.LogWarning("NO SE HAN PODIDO ENCONTRAR CASAS NI OBJETOS, ASEGÚRATE DE QUE TENGAN LA ETIQUETA ASIGNADA");
		}
	}

	[Button]
	public void ToggleProps()
	{
		if (allProps != null && allProps.Length > 0)
		{
			areActiveProps = !areActiveProps;

			PlayerController playerController = FindObjectOfType<PlayerController>();

			foreach (GameObject prop in playerController.hiddenProps)
			{
				//building.SetActive(areActive);

				for (int i = 0; i < prop.transform.childCount; i++)
				{
					//Debug.Log(prop.transform.GetChild(i).name);
					if(prop.transform.GetChild(i).GetComponentInChildren<MeshRenderer>())
						prop.transform.GetChild(i).GetComponentInChildren<MeshRenderer>().enabled = areActiveProps;
				}

				if (prop.GetComponent<MeshRenderer>())
					prop.GetComponent<MeshRenderer>().enabled = areActiveProps;

			}
		}
		else
		{
			Debug.LogWarning("NO HAY CASAS NI OBJETOS, ASEGÚRATE DE PULSAR EL BOTÓN FINDBUILDINGS");
		}
	}




}
