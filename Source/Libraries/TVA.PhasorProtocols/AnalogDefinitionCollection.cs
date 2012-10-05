//******************************************************************************************************
//  AnalogDefinitionCollection.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  02/18/2005 - J. Ritchie Carroll
//       Generated original version of source code.
//  09/15/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  10/5/2012 - Gavin E. Holden
//       Added new header and license agreement.
//
//******************************************************************************************************

using System;
using System.Runtime.Serialization;

namespace TVA.PhasorProtocols
{
    /// <summary>
    /// Represents a protocol independent collection of <see cref="IAnalogDefinition"/> objects.
    /// </summary>
    [Serializable()]
    public class AnalogDefinitionCollection : ChannelDefinitionCollectionBase<IAnalogDefinition>
    {
        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="AnalogDefinitionCollection"/> using specified <paramref name="lastValidIndex"/>.
        /// </summary>
        /// <param name="lastValidIndex">Last valid index for the collection (i.e., maximum count - 1).</param>
        /// <remarks>
        /// <paramref name="lastValidIndex"/> is used instead of maximum count so that maximum type values may
        /// be specified as needed. For example, if the protocol specifies a collection with a signed 16-bit
        /// maximum length you can specify <see cref="Int16.MaxValue"/> (i.e., 32,767) as the last valid index
        /// for the collection since total number of items supported would be 32,768.
        /// </remarks>
        public AnalogDefinitionCollection(int lastValidIndex)
            : base(lastValidIndex)
        {
        }

        /// <summary>
        /// Creates a new <see cref="AnalogDefinitionCollection"/> from serialization parameters.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
        /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
        protected AnalogDefinitionCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}