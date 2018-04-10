/**
 * Copyright (c) 2018 Alireza Bahremand
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InterdimensionalTransporter : MonoBehaviour {
    public Texture imageSkybox;
    public bool facingPortal, inNewReality;
    private Material skyboxInstance;
    private Transform smartphone;

	void Start () {        
        facingPortal = inNewReality = false;
        smartphone = Camera.main.transform; // Assign reference to main camera in scene.
        if (skyboxInstance == null) {       // Determine if skybox instance assigned
            createSkyboxMaterial(imageSkybox);  // If not, create skybox material.
        }
        RenderSettings.skybox = skyboxInstance; // Assign skybox.
        // When starting scene, ensure we are outside of portal
        // by setting proper shader values so portal window renders portal scene.
        SetMaterials(false);
	}

    /**
     * This method will take the texture image,
     * instantiate a material for skybox cubemap,
     * and assign shader texture reference for material
     * as the image assigned to this portal object.
     */ 
    public void createSkyboxMaterial(Texture imageToCubemap) {        
        // Assigns cubemap image to skybox material. Skybox material
        // is dynamically instantiated using custom skybox cubemap shader.
        skyboxInstance = new Material(Shader.Find("Custom/SkyboxCubemap"));
        skyboxInstance.SetTexture("_Tex", imageToCubemap);
    }

    /**
     * If full rendering is enabled, set the stencil shader to not equal, 
     * meaning user is inside portal reality. Otherwise if set to Equal,
     * user is outside of portal reality, so portal plane should render 
     * the reality awaiting.
     */ 
    public void SetMaterials(bool fullRendering) {
        // Set to var because this is a enumerate shader type, and we use it as so 
        // in the foreach loop.
        var stencilTest = fullRendering ? CompareFunction.NotEqual : CompareFunction.Equal;
        skyboxInstance.SetInt("_StencilTest", (int)stencilTest);
    }

    /*
     * This method validates whether the user is in front of the portal after
     * upon collision with portal. This allows for bidirectional rendering of portal
     * path. So should the user walk around the portal and face it again, it properly
     * renders corresponding reality.
     */ 
    private bool isUserInFrontOfPortal() {
        Vector3 tempPosition = transform.InverseTransformPoint(smartphone.position);
        return (tempPosition.z >= 0) ? true : false;
    }

    /*
     * As soon as user collides with portal, this event is fired off.
     */ 
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("MainCamera")) {
            facingPortal = isUserInFrontOfPortal();             
        }
    }

    /*
     * Whilst user is within point of collision.
     */ 
    void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("MainCamera")) {
            // Is user currently facing portal proceeding after collision enter.
            bool isInFront = isUserInFrontOfPortal();
            if ((isInFront && !facingPortal) || (facingPortal && !isInFront)) {
                // Toggle state of base reality.
                inNewReality = !inNewReality;
                SetMaterials(inNewReality);
            }
            // Update based on collision state whether or not
            // user is facing portal or moved away.
            facingPortal = isInFront;
        }
    }


    /**
     * Method allows for setting skybox instance outside of this class.
     * By making method public, materials can be assigned for dynamic skyboxes
     * should the need ever arise. But this would still essentially allow for single
     * rendered skybox instances.
     * There can only be at most one skybox assigned, so every portal
     * will have one skybox material assigned. It's best to delete and assign
     * portals when needed so skybox rendering is in sync with portal of interest.
     */
    public void setSkybox() {
        //Debug.Log("Setting skybox image");
        RenderSettings.skybox = this.skyboxInstance;
    }

    /**
     * Method override for calling outside of class.
     */ 
    public void setSkybox(Material skyboxMaterial) {
        RenderSettings.skybox = skyboxMaterial;
    }

    private void OnDestroy() {
        SetMaterials(true);
    }
}
