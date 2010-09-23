﻿//******************************************************************************************************
//  SqlServerDatabaseSetupScreen.xaml.cs - Gbtc
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
//  09/09/2010 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data;
using System.Reflection;
using TVA;

namespace ConfigurationSetupUtility
{
    /// <summary>
    /// Interaction logic for SqlServerDatabaseSetupScreen.xaml
    /// </summary>
    public partial class SqlServerDatabaseSetupScreen : UserControl, IScreen
    {

        #region [ Members ]

        // Fields

        private SqlServerSetup m_sqlServerSetup;
        private Dictionary<string, object> m_state;
        private Button m_advancedButton;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="SqlServerDatabaseSetupScreen"/> class.
        /// </summary>
        public SqlServerDatabaseSetupScreen()
        {
            m_sqlServerSetup = new SqlServerSetup();
            InitializeComponent();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the screen to be displayed when the user clicks the "Next" button.
        /// </summary>
        public IScreen NextScreen
        {
            get
            {
                IScreen readyScreen;

                if (!State.ContainsKey("readyScreen"))
                    State.Add("readyScreen", new SetupReadyScreen());

                readyScreen = State["readyScreen"] as IScreen;

                return readyScreen;
            }
        }

        /// <summary>
        /// Gets a boolean indicating whether the user can advance to
        /// the next screen from the current screen.
        /// </summary>
        public bool CanGoForward
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a boolean indicating whether the user can return to
        /// the previous screen from the current screen.
        /// </summary>
        public bool CanGoBack
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a boolean indicating whether the user can cancel the
        /// setup process from the current screen.
        /// </summary>
        public bool CanCancel
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a boolean indicating whether the user input is valid on the current page.
        /// </summary>
        public bool UserInputIsValid
        {
            get
            {
                if (string.IsNullOrEmpty(m_hostNameTextBox.Text))
                {
                    MessageBox.Show("Please enter a valid host name for the MySQL instance.");
                    m_hostNameTextBox.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(m_databaseNameTextBox.Text))
                {
                    MessageBox.Show("Please enter a valid database name.");
                    m_databaseNameTextBox.Focus();
                    return false;
                }

                if (m_createNewUserCheckBox.IsChecked.Value && string.IsNullOrEmpty(m_newUserNameTextBox.Text))
                {
                    MessageBox.Show("Please enter a valid user name for the new user.");
                    m_newUserNameTextBox.Focus();
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Collection shared among screens that represents the state of the setup.
        /// </summary>
        public Dictionary<string, object> State
        {
            get
            {
                return m_state;
            }
            set
            {
                m_state = value;
                InitializeState();
            }
        }

        /// <summary>
        /// Allows the screen to update the navigation buttons after a change is made
        /// that would affect the user's ability to navigate to other screens.
        /// </summary>
        public Action UpdateNavigation { get; set; }

        #endregion

        #region [ Methods ]

        // Initialize the state keys to their default values.
        private void InitializeState()
        {
            if (m_state != null)
            {
                bool existing = Convert.ToBoolean(m_state["existing"]);
                bool migrate = existing && Convert.ToBoolean(m_state["updateConfiguration"]);
                Visibility newUserVisibility = (existing && !migrate) ? Visibility.Collapsed : Visibility.Visible;
                string newDatabaseMessage = "Please enter the following information about the database you would like to create.";
                string oldDatabaseMessage = "Please enter the following information about your existing database.";

                m_state["sqlServerSetup"] = m_sqlServerSetup;
                m_sqlServerSetup.HostName = m_hostNameTextBox.Text;
                m_sqlServerSetup.DatabaseName = m_databaseNameTextBox.Text;
                m_createNewUserCheckBox.Visibility = newUserVisibility;
                m_newUserNameLabel.Visibility = newUserVisibility;
                m_newUserPasswordLabel.Visibility = newUserVisibility;
                m_newUserNameTextBox.Visibility = newUserVisibility;
                m_newUserPasswordTextBox.Visibility = newUserVisibility;
                m_sqlServerDatabaseInstructionTextBlock.Text = (!existing || migrate) ? newDatabaseMessage : oldDatabaseMessage;

                if (!m_state.ContainsKey("sqlServerDataProviderString"))
                    m_state.Add("sqlServerDataProviderString", "AssemblyName={System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089}; ConnectionType=System.Data.SqlClient.SqlConnection; AdapterType=System.Data.SqlClient.SqlDataAdapter");

                if (!m_state.ContainsKey("createNewSqlServerUser"))
                    m_state.Add("createNewSqlServerUser", m_createNewUserCheckBox.IsChecked.Value);

                if (!m_state.ContainsKey("newSqlServerUserName"))
                    m_state.Add("newSqlServerUserName", m_newUserNameTextBox.Text);

                if (!m_state.ContainsKey("newSqlServerUserPassword"))
                    m_state.Add("newSqlServerUserPassword", m_newUserPasswordTextBox.Password);

                if (!m_state.ContainsKey("encryptSqlServerConnectionStrings"))
                    m_state.Add("encryptSqlServerConnectionStrings", false);

                m_databaseNameTextBox.Text = migrate ? "openPDCv2" : "openPDC";
            }
        }

        // Occurs when the screen is made visible or invisible.
        private void SqlServerDatabaseSetupScreen_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (m_advancedButton == null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(this);
                Window mainWindow;

                while (parent != null && !(parent is Window))
                    parent = VisualTreeHelper.GetParent(parent);

                mainWindow = parent as Window;
                m_advancedButton = (mainWindow == null) ? null : mainWindow.FindName("m_advancedButton") as Button;
            }

            if (m_advancedButton != null)
            {
                if (IsVisible)
                {
                    m_advancedButton.Visibility = Visibility.Visible;
                    m_advancedButton.Click += AdvancedButton_Click;
                }
                else
                {
                    m_advancedButton.Visibility = Visibility.Collapsed;
                    m_advancedButton.Click -= AdvancedButton_Click;
                }
            }
        }

        // Occurs when the user changes the host name of the SQL Server instance.
        private void HostNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_hostNameTextBox.Text = m_hostNameTextBox.Text.Trim();
            m_sqlServerSetup.HostName = m_hostNameTextBox.Text;
        }

        // Occurs when the user changes the name of the database.
        private void DatabaseNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_sqlServerSetup.DatabaseName = m_databaseNameTextBox.Text;
        }

        // Occurs when the user changes the administrator user name.
        private void AdminUserNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string adminUserName = m_adminUserNameTextBox.Text;
            m_sqlServerSetup.UserName = adminUserName;
        }

        // Occurs when the user changes the administrator password.
        private void AdminPasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string adminPassword = m_adminPasswordTextBox.Password;
            m_sqlServerSetup.Password = adminPassword;
        }

        // Occurs when the user chooses to test their database connection.
        private void TestConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            IDbConnection connection = null;
            Dictionary<string, string> settings;
            string assemblyName, connectionTypeName, adapterTypeName;
            Assembly assembly;
            Type connectionType, adapterType;
            string dataProviderString;
            string databaseName = null;

            try
            {
                databaseName = m_sqlServerSetup.DatabaseName;
                m_sqlServerSetup.DatabaseName = null;

                dataProviderString = m_state["sqlServerDataProviderString"].ToString();
                settings = dataProviderString.ParseKeyValuePairs();
                assemblyName = settings["AssemblyName"].ToNonNullString();
                connectionTypeName = settings["ConnectionType"].ToNonNullString();
                adapterTypeName = settings["AdapterType"].ToNonNullString();

                if (string.IsNullOrEmpty(connectionTypeName))
                    throw new InvalidOperationException("Database connection type was not defined.");

                if (string.IsNullOrEmpty(adapterTypeName))
                    throw new InvalidOperationException("Database adapter type was not defined.");

                assembly = Assembly.Load(new AssemblyName(assemblyName));
                connectionType = assembly.GetType(connectionTypeName);
                adapterType = assembly.GetType(adapterTypeName);

                connection = (IDbConnection)Activator.CreateInstance(connectionType);
                connection.ConnectionString = m_sqlServerSetup.ConnectionString;
                connection.Open();

                MessageBox.Show("Database connection succeeded.");
            }
            catch
            {
                string failMessage = "Database connection failed."
                    + " Please check your username and password."
                    + " Additionally, you may need to modify your connection under advanced settings.";

                MessageBox.Show(failMessage);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();

                if (databaseName != null)
                    m_sqlServerSetup.DatabaseName = databaseName;
            }
        }

        // Occurs when the user chooses to create a new database user.
        private void CreateNewUserCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (m_state != null)
                m_state["createNewSqlServerUser"] = true;
        }

        // Occurs when the user chooses not to create a new database user.
        private void CreateNewUserCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (m_state != null)
                m_state["createNewSqlServerUser"] = false;
        }

        // Occurs when the user changes the user name of the new database user.
        private void NewUserNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (m_state != null)
                m_state["newSqlServerUserName"] = m_newUserNameTextBox.Text;
        }

        // Occurs when the user changes the password of the new database user.
        private void NewUserPasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (m_state != null)
                m_state["newSqlServerUserPassword"] = m_newUserPasswordTextBox.Password;
        }

        // Occurs when the user clicks the "Advanced..." button.
        private void AdvancedButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_state != null)
            {
                string password = m_sqlServerSetup.Password;
                string dataProviderString = m_state["sqlServerDataProviderString"].ToString();
                bool encrypt = Convert.ToBoolean(m_state["encryptSqlServerConnectionStrings"]);
                string connectionString;
                AdvancedSettingsWindow advancedWindow;

                m_sqlServerSetup.Password = null;
                connectionString = m_sqlServerSetup.ConnectionString;
                advancedWindow = new AdvancedSettingsWindow(connectionString, dataProviderString, encrypt);
                advancedWindow.Owner = App.Current.MainWindow;

                if (advancedWindow.ShowDialog() == true)
                {
                    m_sqlServerSetup.ConnectionString = advancedWindow.ConnectionString;
                    m_state["sqlServerDataProviderString"] = advancedWindow.DataProviderString;
                    m_state["encryptSqlServerConnectionStrings"] = advancedWindow.Encrypt;
                }

                if (string.IsNullOrEmpty(m_sqlServerSetup.Password))
                    m_sqlServerSetup.Password = password;

                m_hostNameTextBox.Text = m_sqlServerSetup.HostName;
                m_databaseNameTextBox.Text = m_sqlServerSetup.DatabaseName;
                m_adminUserNameTextBox.Text = m_sqlServerSetup.UserName;
                m_adminPasswordTextBox.Password = m_sqlServerSetup.Password;
            }
        }

        #endregion
    }
}
