using UnityEngine;
using System.Collections;

public class CubeBehaviour : Bolt.EntityEventListener<ICubeState> {

    public GameObject[] WeaponObjects; 

    public override void Attached()
    {
        state.CubeTransform.SetTransforms(transform);

        if (entity.isOwner)
        {
            state.CubeColor = new Color(Random.value, Random.value, Random.value);

            // Setup weapons on the owner
            for (int i = 0; i < state.WeaponsArray.Length; ++i)
            {
                state.WeaponsArray[i].WeaponID = Random.Range(0, WeaponObjects.Length);
                state.WeaponsArray[i].WeaponAmmo = Random.Range(50, 100); 
            }

            // set default no weapon up so = -1
            state.WeaponActiveIndex = -1; 
        }

        state.AddCallback("CubeColor", ColorChanged);

        //setup a callback for whenever the index changes
        state.AddCallback("WeaponActiveIndex", WeaponActiveIndexChanged); 
    }

    void WeaponActiveIndexChanged()
    {
        for (int i = 0; i < WeaponObjects.Length; ++i)
        {
            WeaponObjects[i].SetActive(false); 
        }
        if (state.WeaponActiveIndex >= 0)
        {
            int objectId = state.WeaponsArray[state.WeaponActiveIndex].WeaponID;
            WeaponObjects[objectId].SetActive(true);
        }
    }

    void ColorChanged()
    {
        GetComponent<Renderer>().material.color = state.CubeColor;
    }


    public override void SimulateOwner()
    {
        var speed = 4f;
        var movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) { movement.z += 1; }
        if (Input.GetKey(KeyCode.S)) { movement.z -= 1; }
        if (Input.GetKey(KeyCode.A)) { movement.x -= 1; }
        if (Input.GetKey(KeyCode.D)) { movement.x += 1; }

        //Polling for weapon selection
        if (Input.GetKeyDown(KeyCode.Alpha1)) state.WeaponActiveIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) state.WeaponActiveIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) state.WeaponActiveIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha0)) state.WeaponActiveIndex = -1;

        if (movement != Vector3.zero)
        {
            transform.position = transform.position + (movement.normalized * speed * BoltNetwork.frameDeltaTime);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            var flash = FlashColorEvent.Create(entity);
            flash.FlashColor = Color.red;
            flash.Send();
        }
    }

    float resetColorTime;

    public override void OnEvent(FlashColorEvent evnt)
    {
        resetColorTime = Time.time + 0.2f;
        GetComponent<Renderer>().material.color = evnt.FlashColor; 
    }

    void Update()
    {
        if (resetColorTime < Time.time)
        {
            GetComponent<Renderer>().material.color = state.CubeColor; 
        }
    }

    void OnGUI()
    {
        if (entity.isOwner)
        {
            GUI.color = state.CubeColor;
            GUILayout.Label("@@@");
            GUI.color = Color.white; 
        }
    }
}
