﻿/*
 * Copyright 2014 Systemic Pty Ltd
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Sif.Framework.Model.Persistence;
using Sif.Framework.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sif.Framework.Controller
{

    /// <summary>
    /// This class defines a base Controller containing common operations.
    /// </summary>
    /// <typeparam name="UI">Object type exposed at the presentation/API layer.</typeparam>
    /// <typeparam name="DB">Object type used in the business layer.</typeparam>
    public abstract class SifController<UI, DB> : BaseController
        where UI : new()
        where DB : IPersistable<Guid>, new()
    {
        protected ISifService<UI, DB> service;

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="service">Service used for managing conversion between the object types.</param>
        public SifController(ISifService<UI, DB> service)
        {
            this.service = service;
        }

        /// <summary>
        /// DELETE api/{controller}/{id}
        /// </summary>
        /// <param name="id">Identifier of the object to delete.</param>
        public virtual void Delete(Guid id)
        {

            if (!VerifyAuthorisationHeader(Request.Headers.Authorization))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            try
            {
                UI item = service.Retrieve(id);

                if (item == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                else
                {
                    service.Delete(id);
                }

            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }

        /// <summary>
        /// GET api/{controller}/{id}
        /// </summary>
        /// <param name="id">Identifier of the object to retrieve.</param>
        /// <returns>Object with that identifier.</returns>
        public virtual UI Get(Guid id)
        {

            if (!VerifyAuthorisationHeader(Request.Headers.Authorization))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            UI item;

            try
            {
                item = service.Retrieve(id);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }

        /// <summary>
        /// GET api/{controller}
        /// </summary>
        /// <returns>All objects.</returns>
        public virtual ICollection<UI> Get()
        {

            if (!VerifyAuthorisationHeader(Request.Headers.Authorization))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            ICollection<UI> items;

            try
            {
                items = service.Retrieve();
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return items;
        }

        /// <summary>
        /// POST api/{controller}
        /// </summary>
        /// <param name="item">Object to create.</param>
        /// <returns>HTTP response message indicating success or failure.</returns>
        public virtual HttpResponseMessage Post(UI item)
        {

            if (!VerifyAuthorisationHeader(Request.Headers.Authorization))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            
            HttpResponseMessage responseMessage = null;

            try
            {
                Guid id = service.Create(item);
                responseMessage = Request.CreateResponse<UI>(HttpStatusCode.Created, item);
                string uri = Url.Link("DefaultApi", new { id = id });
                responseMessage.Headers.Location = new Uri(uri);
            }
            catch (Exception)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return responseMessage;
        }

        /// <summary>
        /// PUT api/{controller}/{id}
        /// </summary>
        /// <param name="id">Identifier for the object to update.</param>
        /// <param name="item">Object to update.</param>
        public virtual void Put(Guid id, UI item)
        {

            if (!VerifyAuthorisationHeader(Request.Headers.Authorization))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            try
            {

                if (ModelState.IsValid)
                {
                    UI existingItem = service.Retrieve(id);

                    if (existingItem == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                    else
                    {
                        service.Update(item);
                    }

                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }

    }

}
