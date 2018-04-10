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

public class IdentifyPortal : MonoBehaviour {
    private Camera arKitCamera;     // Reference camera itself.
    private RaycastHit cameraRayHit;
    private Ray arCameraRay;

	void Start () {
        arKitCamera = gameObject.GetComponent<Camera>();
	}
	
	/**
	 * Every frame a raycast is set out to detect if a line shooting straight
	 * from the line of sight for arCamera is colliding with a game object.
	 * The tag of interest is "Portal" objects.
	 */ 
	void Update () {
        if (Physics.Raycast(arKitCamera.transform.position,
                            arKitCamera.transform.forward,
                            out cameraRayHit,
                            Mathf.Infinity)) {
            if (cameraRayHit.collider.gameObject.CompareTag("Portal")) {
                //Debug.Log("Collision");
                /**
                 * Grab glanced at portal and set the unity skybox to be corresponding
                 * portal in users line of sight.
                 */ 
                cameraRayHit.collider.gameObject.GetComponent<InterdimensionalTransporter>().setSkybox();
            }
        }
	}

}
