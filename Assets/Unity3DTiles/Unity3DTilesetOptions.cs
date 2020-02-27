﻿/*
 * Copyright 2018, by the California Institute of Technology. ALL RIGHTS 
 * RESERVED. United States Government Sponsorship acknowledged. Any 
 * commercial use must be negotiated with the Office of Technology 
 * Transfer at the California Institute of Technology.
 * 
 * This software may be subject to U.S.export control laws.By accepting 
 * this software, the user agrees to comply with all applicable 
 * U.S.export laws and regulations. User has the responsibility to 
 * obtain export licenses, or other export authority as may be required 
 * before exporting such information to foreign countries or providing 
 * access to foreign persons.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Unity3DTiles
{
    [Serializable] //Serializable so it will show up in Unity editor inspector
    public class Unity3DTilesetOptions
    {
        //Options unique to a single tileset 

        [Tooltip("Unique name of tileset. Defaults to Url if null or empty.")]
        public string Name = null;

        [Tooltip("Full path URL to the tileset. Can be a local file or url as long as it is a full path, or can start with StreamingAssets.")]
        public string Url = null;

        [Tooltip("Whether tileset is initially visible.")]
        public bool Show = true;

        public UnityEngine.Rendering.ShadowCastingMode ShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        public bool RecieveShadows = true;

        public bool CreateColliders = true;

        [Tooltip("Controls the level of detail the tileset will be loaded to by specifying the allowed amount of on screen geometric error allowed in pixels")]
        public double MaximumScreenSpaceError = 16;

        [Tooltip("Controls what parent tiles will be skipped when loading a tileset.  This number will be multipled by MaximumScreenSpaceError and any tile with an on screen error larger than this will be skipped by the loading and rendering algorithm")]
        public double SkipScreenSpaceErrorMultiplier = 16;

        [Tooltip("If a tile is in view and needs to be rendered, also load its siblings even if they are not visible.  Especially useful when using colliders so that raycasts outside the users field of view can succeed.  Increases load time and number of tiles that need to be stored in memory.")]
        public bool LoadSiblings = true;

        [JsonConverter(typeof(Matrix4x4Converter))]
        public Matrix4x4 Transform = Matrix4x4.identity;

        [Tooltip("Max child depth that we should render. If this is zero, disregard")]
        public int MaxDepth = 0;

        [Header("GLTF Loader Settings")]
        public bool GLTFMultithreadedLoad = true;

        public int GLTFMaximumLOD = 300;

        [JsonIgnore]
        public Shader GLTFShaderOverride;

        [Header("Debug Settings")]
        public bool DebugDrawBounds = false;

        [JsonIgnore]
        public System.Func<Unity3DTile, float> TilePriority = new System.Func<Unity3DTile, float>(tile =>
        {
            return (float)(tile.Depth - 1.0 / tile.FrameState.DistanceToCamera);
        });
    }

    [Serializable] //Serializable so it will show up in Unity editor inspector
    public class Unity3DTilesetSceneOptions
    {
        //Options shared between tilesets in a scene

        [Tooltip("Controls how many colliders can be created per frame, this can be an expensive operation on some platforms.  Increasing this number will decrese load time but may increase frame lurches when loading tiles.")]
        public int MaximumTilesToProcessPerFrame = 1;

        [Tooltip("Sets the target maximum number of tiles that can be loaded into memory at any given time.  Beyond this limit, unused tiles will be unloaded as new requests are made.")]
        public int LRUCacheTargetSize = 600;

        [Tooltip("Sets the maximum number of tiles (hard limit) that can be loaded into memory at any given time. Requests that would exceed this limit fail.")]
        public int LRUCacheMaxSize = 700;

        [Tooltip("Controls the maximum number of unused tiles that will be unloaded at a time when the cache is full.  This is specified as a ratio of the LRUMaxCacheSize. For example, if this is set to 0.2 and LRUMaxCacheSize is 600 then at most we will unload 120 (0.2*600) tiles in a single frame.")]
        public float LRUMaxFrameUnloadRatio = 0.2f;

        public int MaxConcurrentRequests = 6;

        [JsonIgnore]
        public List<Camera> ClippingCameras = new List<Camera>();

        [JsonIgnore]
        public Shader GLTFShaderOverride;

        [Header("Camera Settings")]
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 DefaultCameraPosition = new Vector3(0, 0, -30);
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 DefaultCameraRotation = Vector3.zero;
    }
}
