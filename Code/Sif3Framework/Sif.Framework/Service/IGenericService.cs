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
using System.Collections.Generic;

namespace Sif.Framework.Service
{

    public interface IGenericService<T, PK> where T : IPersistable<PK>
    {

        PK Create(T obj);

        void Create(IEnumerable<T> objs);

        void Delete(PK id);

        void Delete(T obj);

        void Delete(IEnumerable<T> objs);

        T Retrieve(PK id);

        ICollection<T> Retrieve(T obj);

        ICollection<T> Retrieve();

        void Update(T obj);

        void Update(IEnumerable<T> objs);

    }

}
