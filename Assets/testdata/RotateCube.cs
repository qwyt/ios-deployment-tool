using UnityEngine;
using System.Collections;

public class RotateCube : MonoBehaviour {

	private IEnumerator Start()
	{
		MeshRenderer mr = this.gameObject.GetComponent<MeshRenderer>();
		mr.sharedMaterial.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),1.0f); 

		yield return new WaitForSeconds(Random.Range(0.1f,1.0f));

		for(;;)
		{
			for(int i=0;i<10;i++)
			{
				this.transform.position += new Vector3(0,0.2f,0);
				yield return 0;
			}

			for(int i=0;i<10;i++)
			{
				this.transform.position += new Vector3(0,-0.2f,0);
				yield return 0;
			}
		}
	}

	private void Update()
	{
		this.transform.Rotate(new Vector3(5,5,5));
	}
}
