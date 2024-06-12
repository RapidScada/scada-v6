// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scada.Lang;
using Scada.Web.Api;
using Scada.Web.Authorization;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMimic.Controllers;
using Scada.Web.Plugins.PlgMimic.MimicModel;
using Scada.Web.Plugins.PlgMimic.Models;
using Scada.Web.Plugins.PlgMimicEditor.Code;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMimicEditor.Controllers
{
    /// <summary>
    /// Represents a web API for loading mimic diagrams.
    /// <para>Представляет веб-API для загрузки мнемосхем.</para>
    /// </summary>
    [ApiController]
    [Route("Api/MimicEditor/Loader/[action]")]
    [Authorize(Policy = PolicyName.Administrators)]
    [TypeFilter(typeof(MimicLockFilter))]
    [CamelCaseJsonFormatter]
    public class LoaderApiController(
        IWebContext webContext,
        EditorManager editorManager) : ControllerBase, IMimicApiController
    {
        /// <summary>
        /// Gets the mimic properties.
        /// </summary>
        public Dto<MimicPacket> GetMimicProperties(long key)
        {
            try
            {
                if (editorManager.FindMimic(key, out MimicInstance mimicInstance, out string errMsg))
                {
                    return Dto<MimicPacket>.Success(new MimicPacket
                    {
                        MimicStamp = key,
                        Dependencies = mimicInstance.Mimic.Dependencies,
                        Document = mimicInstance.Mimic.Document
                    });
                }
                else
                {
                    return Dto<MimicPacket>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetMimicProperties)));
                return Dto<MimicPacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets a range of components.
        /// </summary>
        public Dto<ComponentPacket> GetComponents(long key, int index, int count)
        {
            try
            {
                if (editorManager.FindMimic(key, out MimicInstance mimicInstance, out string errMsg))
                {
                    List<Component> components = [];
                    int currentIndex = 0;
                    bool endReached = true;
                    
                    foreach (Component component in mimicInstance.Mimic.EnumerateComponents())
                    {
                        if (currentIndex++ >= index)
                        {
                            if (components.Count < count)
                                components.Add(component);

                            if (components.Count >= count)
                            {
                                endReached = false;
                                break;
                            }
                        }
                    }
                    
                    return Dto<ComponentPacket>.Success(new ComponentPacket
                    {
                        MimicStamp = key,
                        EndOfComponents = endReached,
                        Components = components
                    });
                }
                else
                {
                    return Dto<ComponentPacket>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetComponents)));
                return Dto<ComponentPacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets a range of images.
        /// </summary>
        public Dto<ImagePacket> GetImages(long key, int index, int count, int size)
        {
            try
            {
                if (editorManager.FindMimic(key, out MimicInstance mimicInstance, out string errMsg))
                {
                    List<Image> images = [];
                    int currentIndex = index;
                    int imageCount = mimicInstance.Mimic.Images.Count;
                    int counter = 0;
                    int totalSize = 0;

                    while (currentIndex < imageCount && counter < count)
                    {
                        if (currentIndex >= 0)
                        {
                            Image image = mimicInstance.Mimic.Images.GetValueAtIndex(currentIndex);

                            if (images.Count == 0 || totalSize + image.DataSize <= size)
                            {
                                images.Add(image);
                                totalSize += image.DataSize;
                            }
                            else
                            {
                                break;
                            }
                        }

                        currentIndex++;
                        counter++;
                    }

                    return Dto<ImagePacket>.Success(new ImagePacket
                    {
                        MimicStamp = key,
                        EndOfImages = currentIndex >= imageCount,
                        Images = images
                    });
                }
                else
                {
                    return Dto<ImagePacket>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetImages)));
                return Dto<ImagePacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets the faceplate.
        /// </summary>
        public Dto<FaceplatePacket> GetFaceplate(long key, string typeName)
        {
            try
            {
                if (editorManager.FindMimic(key, out MimicInstance mimicInstance, out string errMsg))
                {
                    if (mimicInstance.Mimic.Faceplates.TryGetValue(typeName, out Faceplate faceplate))
                    {
                        return Dto<FaceplatePacket>.Success(new FaceplatePacket
                        {
                            MimicStamp = key,
                            Document = faceplate.Document,
                            Components = faceplate.Components,
                            Images = faceplate.Images.Values
                        });
                    }
                    else
                    {
                        return Dto<FaceplatePacket>.Fail(Locale.IsRussian ?
                            "Фейсплейт не найден." :
                            "Faceplate not found.");
                    }
                }
                else
                {
                    return Dto<FaceplatePacket>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetFaceplate)));
                return Dto<FaceplatePacket>.Fail(ex.Message);
            }
        }
    }
}
