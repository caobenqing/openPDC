﻿//******************************************************************************************************
//  OutputStreamDeviceAnalogs.cs - Gbtc
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
//  08/08/2011 - Aniket Salver
//       Generated original version of source code.
//  08/30/2011 - Aniket Salver
//       Added few properties which helps in biding.
//
//******************************************************************************************************

using openPDC.UI.DataModels;
using TimeSeriesFramework.UI;
using System.Collections.Generic;

namespace openPDC.UI.ViewModels
{
    // <summary>
    /// Class to hold bindable <see cref="OutputStreamDeviceAnalog"/> collection and selected OutputStreamDeviceAnalog for UI.
    /// </summary>
    internal class OutputStreamDeviceAnalogs : PagedViewModelBase<OutputStreamDeviceAnalog, int>
    {
        #region[Members]

        // Fields

        private Dictionary<int, string> m_typeLookupList;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets flag that determines if <see cref="PagedViewModelBase{T1, T2}.CurrentItem"/> is a new record.
        /// </summary>
        public override bool IsNewRecord
        {
            get
            {
                return CurrentItem.ID == 0;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> Type collection of type defined in the database.
        /// </summary>
        public Dictionary<int, string> TypeLookupList
        {
            get
            {
                return m_typeLookupList;
            }
        }

        #endregion

        #region [ Constructor ]

        /// <summary>
        /// Creates an instance of <see cref="OutputStreamDeviceAnalogs "/> class.
        /// </summary>
        /// <param name="itemsPerPage">Integer value to determine number of items per page.</param>
        /// <param name="autoSave">Boolean value to determine is user changes should be saved automatically.</param>
        public OutputStreamDeviceAnalogs(int itemsPerPage, bool autoSave = true)
            : base(itemsPerPage, autoSave)
        {
            m_typeLookupList = new Dictionary<int, string>();
            m_typeLookupList.Add(0, "Single point-on-wave");
            m_typeLookupList.Add(1, "RMS of analog input");
            m_typeLookupList.Add(2, "Peak of analog input");

        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the primary key value of the <see cref="PagedViewModelBase{T1, T2}.CurrentItem"/>.
        /// </summary>
        /// <returns>The primary key value of the <see cref="PagedViewModelBase{T1, T2}.CurrentItem"/>.</returns>
        public override int GetCurrentItemKey()
        {
            return CurrentItem.ID;
        }

        /// <summary>
        /// Gets the string based named identifier of the <see cref="PagedViewModelBase{T1, T2}.CurrentItem"/>.
        /// </summary>
        /// <returns>The string based named identifier of the <see cref="PagedViewModelBase{T1, T2}.CurrentItem"/>.</returns>
        public override string GetCurrentItemName()
        {
            return CurrentItem.Label;
        }

        #endregion
    }
}
