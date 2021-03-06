//-----------------------------------------------------------------------
// <copyright file="AugmentedImageVisualizer.cs" company="Google LLC">
//
// Copyright 2018 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;

    /// <summary>
    /// Uses 4 frame corner objects to visualize an AugmentedImage.
    /// </summary>
    public class AugmentedImageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;

        /// <summary>
        /// 目的地マーカーオブジェクト
        /// </summary>
        public GameObject DistinationMarker;

        public void Update()
        {
            if (Image == null || Image.TrackingState != TrackingState.Tracking)
            {
                DistinationMarker.SetActive(false);
                return;
            }

            float halfWidth = Image.ExtentX / 2;
            float halfHeight = Image.ExtentZ / 2;
            DistinationMarker.transform.localPosition = (new Vector3(0,0,0)　) + (new Vector3(0,0,0) * 1f ) ;

            /* float halfWidth = Image.ExtentX / 2;
            float halfHeight = Image.ExtentZ / 2;
            FrameLowerLeft.transform.localPosition =
                (halfWidth * (Vector3.left * 5)) + (halfHeight * Vector3.back);
            FrameLowerRight.transform.localPosition =
                (halfWidth * Vector3.right) + (halfHeight * Vector3.back);
            FrameUpperLeft.transform.localPosition =
                (halfWidth * Vector3.left) + (halfHeight * Vector3.forward);
            FrameUpperRight.transform.localPosition =
                (halfWidth * Vector3.right) + (halfHeight * Vector3.forward); */

            DistinationMarker.SetActive(true);
        }
    }
}
